public class GPU
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Memory { get; set; }

    public override string ToString()
    {
        return $"{Name} - {Price} руб. ({Memory}GB)";
    }
} 