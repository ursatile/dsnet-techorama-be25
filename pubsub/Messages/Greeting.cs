namespace Messages;

public class Greeting {
	private static int number = 0;
	private readonly int myNumber = number++;

	public DateTimeOffset GreetingTime { get; set; } = DateTimeOffset.Now;
	public string Name { get; set; } = String.Empty;

	public override string ToString()
		=> $"Greeting #{myNumber} from {Name} at {GreetingTime:O}";
}
