namespace ElevatorChallenge.Dto.Model
{ 
    public record ErrorHandling
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
    }
}