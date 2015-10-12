using System;

public class Actor
{
    public Attribute Stats;

    public int Id;

    public int Level;

    public string Description;

    public int HP;

    public int MP;
	
	public Actor ()
	{
        Stats = new Attribute();

	}
}
