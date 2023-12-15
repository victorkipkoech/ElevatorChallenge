using ElevatorChallenge.Dto.Model;
using ElevatorChallenge.Service;

namespace ElevatorChallenge.Repository
{
    public interface IElevatorRepository
    {
        List<ElevatorService> Elevators { get; set; }
        ErrorHandling AddElevator(ElevatorService elevator);
        (ErrorHandling ErrorHandling, ElevatorService? Elevator) RequestElevator(int floor);
    }
}