using ElevatorChallenge.Repository;
using System.Drawing;

namespace ElevatorChallenge.xUnitTest
{
    public class ElevatorUnitTest
    {
        ElevatorRepository repo;
        public ElevatorUnitTest()
        {
            repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL2", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL3", new List<int> { { 2 }, { 4 }, { 6 }, { 8 } }, 200, 2));
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL4", new List<int> { { 2 }, { 4 }, { 6 }, { 8 } }, 200, 2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9)]
        public void TakeAnElevator_UnsuportFloor_MustReturnFalse(int floor)
        {
            //Arrange

            //Act
            var (result, _) = repo.RequestElevator(floor);

            //Assert
            Assert.False(result.Successful);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void TakeAnElevator_Floor_1_3_5_MustReturn_EL1OrEL2(int floor)
        {
            //Arrange

            //Act
            var (result, elevator) = repo.RequestElevator(floor);

            //Assert
            Assert.True(result.Successful && (elevator.Name.Equals("EL1") || elevator.Name.Equals("EL2")));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(8)]
        public void AddPersonForFloor2_4_6_8_ThenReturnShouldBeFalse(int floor)
        {
            //Arrange
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            var (result, elevator) = repo.RequestElevator(1);

            //Act
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 70,
                FloorToGo = floor
            });

            //Assert
            Assert.False(resultAddPerson.Successful);
        }

        [Fact]
        public void AddPersonForFloor1WithWeightMoreThanStandard_ThenReturnShouldBeFalse()
        {
            //Arrange
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            var (result, elevator) = repo.RequestElevator(1);
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = 3
            });

            //Act
            resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 70,
                FloorToGo = 5
            });

            //Assert
            Assert.False(resultAddPerson.Successful);
        }

        [Fact]
        public void AddPersonForFloor1WithDestinationFloor1_ThenReturnShouldBeFalse()
        {
            //Arrange
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            var (result, elevator) = repo.RequestElevator(1);

            //Act
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = 1
            });

            //Assert
            Assert.False(resultAddPerson.Successful);
        }

        [Fact]
        public void Move_FromFloor1ToFloor3_ShouldReturnFloor3()
        {
            //Arrange
            var floorToGo = 3;
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            var (result, elevator) = repo.RequestElevator(1);
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = floorToGo
            });

            //Act
            var resultMove = elevator.Move();
            //Assert
            Assert.Equal(floorToGo.ToString(), elevator.CurrentFloor.ToString());
        }

        [Fact]
        public void Move_FromFloor1ToFloor7_ShouldReturnFloor7AndDirectionDown()
        {
            //Arrange
            var floorToGo = 7;
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            var (result, elevator) = repo.RequestElevator(1);
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = floorToGo
            });

            //Act
            var resultMove = elevator.Move();
            //Assert
            Assert.True(elevator.CurrentFloor == floorToGo && elevator.Direction == ElevatorChallenge.Dto.Enums.ElevatorDirection.Down);
        }

        [Fact]
        public void Move_FromFloor7ToFloor1_ShouldReturnFloor1AndDirectionUp()
        {
            //Arrange
            var floorToGo = 1;
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 7));
            var (result, elevator) = repo.RequestElevator(7);
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = floorToGo
            });

            //Act
            var resultMove = elevator.Move();
            //Assert
            Assert.True(elevator.CurrentFloor == floorToGo && elevator.Direction == ElevatorChallenge.Dto.Enums.ElevatorDirection.Up);
        }

        [Fact]
        public void GetOff_ReturnFalseIfNoBodyIsInTheElevator()
        {
            //Arrange            
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 7));
            var (result, elevator) = repo.RequestElevator(7);

            //Act
            var resultMove = elevator.GetOff();
            //Assert
            Assert.False(resultMove.Successful);
        }

        [Fact]
        public void GetOff_GetOffFromFloor3_ReturnTrueAndTotalPassengerZero()
        {
            //Arrange
            var floorToGo = 3;
            var repo = new ElevatorRepository();
            repo.AddElevator(new ElevatorChallenge.Service.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 7));
            var (result, elevator) = repo.RequestElevator(7);
            var resultAddPerson = elevator.AddPerson(new ElevatorChallenge.Dto.Model.Person
            {
                Weight = 150,
                FloorToGo = floorToGo
            });
            var resultMove = elevator.Move();

            //Act
            var resultGetOff= elevator.GetOff();
            //Assert
            Assert.True(resultGetOff.Successful && elevator.People.Count==0);
        }
    }
}