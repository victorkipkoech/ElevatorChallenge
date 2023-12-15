using System.ComponentModel.DataAnnotations;

namespace ElevatorChallenge.Dto.Model
{
    public interface IPerson {
        int Weight { get; set; }
    }
    public class Person: IPerson
    {
        public Guid Id { get; } = Guid.NewGuid();
        [Range(1,200)]
        public int Weight { get; set; }
        public int FloorToGo { get; set; }
    }
}