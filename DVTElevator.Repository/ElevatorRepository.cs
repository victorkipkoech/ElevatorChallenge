using ElevatorChallenge.Dto.Model;
using ElevatorChallenge.Service;

namespace ElevatorChallenge.Repository
{
    public class ElevatorRepository: IElevatorRepository
    {
        public List<ElevatorService> Elevators { get; set; }= new ();

        public  ErrorHandling AddElevator(ElevatorService elevator) {
            if (string.IsNullOrWhiteSpace(elevator?.Name))
                return new ErrorHandling
                {
                    Message = "Please provide the name for your elevator"
                };
            
            if (Elevators.Any(x => x.Name.Equals(elevator.Name, StringComparison.OrdinalIgnoreCase)))
                return new ErrorHandling
                {
                    Message = "Please select another name for your elevator as the name provided exists."
                };

            Elevators.Add(elevator);
            return new ErrorHandling
            {
                Message = $"Elevator {elevator.Name} is added successfully.",
                Successful = true
            };
        }

        public  (ErrorHandling ErrorHandling, ElevatorService? Elevator) RequestElevator(int floor) {
            if (Elevators.Count == 0)
                return (new ErrorHandling
                {
                    Message = "No elevator is specified."
                }, null);

            var availableElevators = new List<ElevatorService>();
            foreach (var elevator in Elevators)
            {
                var totalWeight = elevator.People.Sum(x => x.Weight);
                if (elevator.WorkingFloors.Any(x => x == floor) && totalWeight <= elevator.MaxWeight)
                    availableElevators.Add(elevator);
            }

            if (availableElevators.Count == 0)
                return (new ErrorHandling
                {
                    Message = $"There is no available elevator for floor {floor}."
                }, null);

            var tmpList = availableElevators.Where(
                x =>
                    (x.Direction == ElevatorChallenge.Dto.Enums.ElevatorDirection.Up && x.CurrentFloor <= floor) ||
                    (x.Direction == ElevatorChallenge.Dto.Enums.ElevatorDirection.Down && x.CurrentFloor >= floor)
            ).ToList();

            if (tmpList.Count == 0)
            {
                tmpList = availableElevators.ToList();
                //return (new ErrorHandling
                //{
                //    Message = $"There is no available elevator for floor {floor}."
                //}, null);
            }

            foreach (var item in tmpList)
            {
                item.Difference = Math.Abs((item.CurrentFloor ?? (floor+1)) - floor);
            }

            var elev = tmpList.OrderBy(x => x.Difference).FirstOrDefault();
            elev.SetCurrentFloor( floor);
            return (new ErrorHandling
            {
                Successful = true,
                Message = $"The elevator<{elev.Name}> has been selected"
            }, elev);

        }
    }
}