using ElevatorChallenge.Dto.Enums;
using ElevatorChallenge.Dto.Model;
using System.Security.Cryptography.X509Certificates;

namespace ElevatorChallenge.Service
{
    public class ElevatorService : IElevatorService
    {
        public string Name { get; }
        public List<int> WorkingFloors { get; }
        public int MaxWeight { get; }
        public int? CurrentFloor { get; private set; }
        public List<Person> People { get; private set; }
        public ElevatorStatus Status { get; }
        public ElevatorDirection Direction { get; private set; }
        public int Difference { get; set; }

        public ElevatorService(string name, List<int> workingFloors, int maxWeight, int currentFloor)
        {
            Name = name;
            WorkingFloors = workingFloors.GroupBy(x => x).Select(x => x.Key).ToList();
            MaxWeight = maxWeight;
            CurrentFloor = currentFloor;
            People = new List<Person>();
        }

        public virtual ErrorHandling GetOff()
        {
            var operating = CheckIfElevatorOperates();
            if (!operating.Successful)
                return operating;

            if (!People.Any())
                return new ErrorHandling
                {
                    Message = "The elevator is empty.",
                };

            if (WorkingFloors is null || !WorkingFloors.Any())
                return new ErrorHandling
                {
                    Message = "No floors specified",
                };

            if (Status != ElevatorStatus.Stopped)
                return new ErrorHandling
                {
                    Message = "Safety issue. We cannot accept removing person while the elevator is moving.",
                };

            var peopleForCurrentFloor = People.Where(x => x.FloorToGo == CurrentFloor).ToList();
            if (peopleForCurrentFloor is null || !peopleForCurrentFloor.Any())
                return new ErrorHandling
                {
                    Message = $"There is no person to get off in floor {CurrentFloor}."
                };

            var index = new Random().Next(0, peopleForCurrentFloor.Count - 1);

            People.Remove(peopleForCurrentFloor[index]);

            return new ErrorHandling
            {
                Message = "A person got off successfully.",
                Successful = true
            };
        }

        public virtual ErrorHandling Move()
        {
            var operating = CheckIfElevatorOperates();
            if (!operating.Successful)
                return operating;

            if (Status != ElevatorStatus.Stopped)
                return new ErrorHandling
                {
                    Message = "The elevator is currently moving.",
                };

            if (Direction == ElevatorDirection.Up)
            {
                var floorToGo = People.Where(x => x.FloorToGo > CurrentFloor)?.OrderBy(x => x.FloorToGo)?.FirstOrDefault()?.FloorToGo;
                if (floorToGo != null)
                    CurrentFloor = floorToGo;
                else
                {
                    floorToGo = People.Where(x => x.FloorToGo < CurrentFloor)?.OrderBy(x => x.FloorToGo)?.FirstOrDefault()?.FloorToGo;
                    if (floorToGo != null)
                        CurrentFloor = floorToGo;
                }
            }
            else
            {
                var floorToGo = People.Where(x => x.FloorToGo < CurrentFloor)?.OrderByDescending(x => x.FloorToGo)?.FirstOrDefault()?.FloorToGo;
                if (floorToGo != null)
                    CurrentFloor = floorToGo;
                else
                {
                    floorToGo = People.Where(x => x.FloorToGo > CurrentFloor)?.OrderByDescending(x => x.FloorToGo)?.FirstOrDefault()?.FloorToGo;
                    if (floorToGo != null)
                        CurrentFloor = floorToGo;
                }
            }

            if (CurrentFloor == WorkingFloors.Max())
                Direction = ElevatorDirection.Down;

            if (CurrentFloor == WorkingFloors.Min())
                Direction = ElevatorDirection.Up;

            return new ErrorHandling
            {
                Message = $"Elevator<{Name}> goes {GetDirection()}",
                Successful = true
            };
        }

        public string GetDirection()
        {
            return Direction switch
            {
                _ when Direction == ElevatorDirection.Up => "up",
                _ when Direction == ElevatorDirection.Down => "down",
                _ => "Error"
            };
        }

        public virtual ErrorHandling AddPerson(Person person)
        {
            var operating = CheckIfElevatorOperates();
            if (!operating.Successful)
                return operating;

            if (WorkingFloors is null || !WorkingFloors.Any())
                return new ErrorHandling
                {
                    Message = "No floors specified",
                };

            if (Status != ElevatorStatus.Stopped)
                return new ErrorHandling
                {
                    Message = "Safety issue. We cannot accept adding person while the elavator is moving.",
                };

            if (!WorkingFloors.Any(x => x == person.FloorToGo))
                return new ErrorHandling
                {
                    Message = $"Limited floor. The elavator cannot stop in floor {person.FloorToGo} or the floor does not exist.",
                };

            var totalCurrentWeight = People.Sum(x => x.Weight);
            if (totalCurrentWeight + person.Weight > MaxWeight)
                return new ErrorHandling
                {
                    Message = "Limited weight. Please use another elavator."
                };

            if (person.FloorToGo == CurrentFloor)
                return new ErrorHandling
                {
                    Message = $"You are in the floor {person.FloorToGo}."
                };

            People.Add(person);

            return new ErrorHandling
            {
                Message = "A person added successfully.",
                Successful = true
            };
        }

        public void SetCurrentFloor(int floor)
        {
            CurrentFloor = floor;
        }

        ErrorHandling CheckIfElevatorOperates()
        {
            return new ErrorHandling
            {
                Successful = Status != ElevatorStatus.Broken,
                Message = Status == ElevatorStatus.Broken ? "Elevator is broken" : ""
            };
        }

    }

}