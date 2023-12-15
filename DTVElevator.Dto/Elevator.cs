using ElevatorChallenge.Dto.Enums;

namespace ElevatorChallenge.Dto.Model
{
    public interface IElevator
    {
        string Name { get; }
        List<int> WorkingFloors { get; }
        List<Person> People { get; }
        /// <summary>
        /// Kg
        /// </summary>
        int MaxWeight { get; }
        int? CurrentFloor { get; }
        ElevatorStatus Status { get; }
        ElevatorDirection Direction{ get; }
        ErrorHandling Move();
        ErrorHandling AddPerson(Person person);
        /// <summary>
        /// Remove a person from the people randomly
        /// </summary>
        /// <returns></returns>
        ErrorHandling GetOff();
    }
}