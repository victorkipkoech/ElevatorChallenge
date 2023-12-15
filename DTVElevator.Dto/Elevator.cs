using ElevatorChallenge.Dto.Enums;

namespace ElevatorChallenge.Dto.Model
{
    public interface IElevator
    {
        string Name { get; }
        List<int> WorkingFloors { get; }
        List<Person> People { get; }
        int MaxWeight { get; }
        int? CurrentFloor { get; }
        ElevatorStatus Status { get; }
        ElevatorDirection Direction{ get; }
        ErrorHandling Move();
        ErrorHandling AddPerson(Person person);
        
        ErrorHandling GetOff();
    }
}