
using ElevatorChallenge.Dto;
using ElevatorChallenge.Repository;
using ElevatorChallenge.Service;

Console.WriteLine("Please use the menu to command the Elevator");
var isEnd = false;
var command= "";
var isExit = false;
IElevatorRepository repo = new ElevatorRepository();
int? elevatorRequestedFromFloor = null;
ElevatorService selectedElevator = null;
List<Menu> menus;
loadMenus();

while (!isEnd) {
    generateMainMenu();
    command = Console.ReadLine();
    switch (command)
    {
        case "1":
            createElevator();
            break;
        case "2":
            listOfElavators();
            break;
        case "3":
            requestAnElevator();
            break;
        case "4":
            addPerson();
            break;
        case "5":
            moveElevator();
            break;
        case "6":
            getOff();
            break;
        case "0":
            isExit = true;
            break;
    }
    if (isExit) break;
}

Console.WriteLine("Bye");

void generateMainMenu()
{
    Console.WriteLine();
    Console.WriteLine("-------Menu-------");
    for (int i = 0; i < menus.Count; i++)
    {
        var menu = menus[i];
        Console.WriteLine($"{menu.Command}. {menu.Name}");
    }
    Console.WriteLine();
    if (selectedElevator is not null)
        WriteMessage($"Elevator {selectedElevator.Name} is selected.", false);

    Console.WriteLine();
}

void createElevator() {
    try
    {
        Console.Write("Elevator Name: ");
        var name = Console.ReadLine();
        Console.Write("Working Floors (Comma sepration): ");
        var _workingFloors = Console.ReadLine();
        var workingFloors = new List<int>();
        var split = _workingFloors.Split(",");
        foreach (var item in split)
        {
            workingFloors.Add(int.Parse(item.Trim()));
        }
        Console.Write("Max Weight: ");
        var maxWeight =int.Parse(Console.ReadLine());
        var currentFloor = workingFloors.OrderBy(x => x).FirstOrDefault();
        var result = repo.AddElevator(new ElevatorChallenge.Service.ElevatorService(name, workingFloors, maxWeight, currentFloor));
        WriteMessage(result.Message, !result.Successful);
    }
    catch (Exception ex)
    {
        WriteMessage(ex.Message, true);
    }
}

void WriteMessage(string message,bool isError) {
    Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.White;
}

void listOfElavators() {
    if (repo.Elevators.Count == 0) {
        WriteMessage("No elevators specified",true);
        return;
    }
    Console.WriteLine($"-----List of Elevators-----");
    foreach (var item in repo.Elevators)
    {
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Direction: {item.GetDirection()}");
        Console.WriteLine($"Current Floor: {item.CurrentFloor}");
        Console.WriteLine($"Max Weight: {item.MaxWeight}");
        Console.WriteLine($"Working Floors: {string.Join(",", item.WorkingFloors)}");
        Console.WriteLine($"People : {item.People.Count}");
        
        for (int i = 0; i < item.People.Count; i++)
        {
            var person = item.People[i];
            Console.WriteLine($"    {i + 1}. {person.Weight}(Kg) person wants to go to floor {person.FloorToGo}");
        }
        
        Console.WriteLine($"Total Weight : {item.People.Sum(x => x.Weight)}");
        Console.WriteLine();
    }
}
void requestAnElevator() {
    try
    {
        if (repo.Elevators.Count == 0)
        {
            WriteMessage("No elevators specified", true);
            return;
        }
        Console.Write("Request an elevator from floor: ");
        var floor =int.Parse( Console.ReadLine());

        var (errorHandling,elevator) = repo.RequestElevator(floor);
        if (!errorHandling.Successful) {
            WriteMessage(errorHandling.Message, true);
            return;
        }
        elevatorRequestedFromFloor= floor;
        selectedElevator = elevator;
    }
    catch (Exception ex) {
        WriteMessage(ex.Message, true);
    }
}

void addPerson() {
    if (selectedElevator is null) {
        WriteMessage("No elevator selected", true);
        return;
    }
    Console.Write("Weight of the person: ");
    var weight=int.Parse(Console.ReadLine());
    Console.Write("Floor to go: ");
    var floor = int.Parse(Console.ReadLine());
    var result = selectedElevator.AddPerson(new ElevatorChallenge.Dto.Model.Person { Weight = weight, FloorToGo = floor });
    WriteMessage(result.Message,!result.Successful);
    return;
}

void moveElevator()
{
    if (selectedElevator is null)
    {
        WriteMessage("No elevator selected", true);
        return;
    }
    var result = selectedElevator.Move();
    WriteMessage(result.Message, !result.Successful);
    return;
}

void getOff()
{
    if (selectedElevator is null)
    {
        WriteMessage("No elevator selected", true);
        return;
    }
    
    var result = selectedElevator.GetOff();
    WriteMessage(result.Message, !result.Successful);
    return;
}

void loadMenus()
{
    menus = new List<Menu>{
    new Menu{
        Command="1",
        Name="Create Elevator"
    },
    new Menu{
        Command="2",
        Name="List of Elevators"
    },
    new Menu{
        Command="3",
        Name="Request an Elevator"
    },
    new Menu{
        Command="4",
        Name="Add Person"
    },
    new Menu{
        Command="5",
        Name="Move Selected Elevator"
    },
    new Menu{
        Command="6",
        Name="Get Off"
    },
    new Menu{
        Command="0",
        Name="Exit"
    }
};
}
