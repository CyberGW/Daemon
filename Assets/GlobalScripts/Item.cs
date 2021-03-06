﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for items that can be equipped to a player, increasing some of their stats
/// </summary>
public abstract class Item
{

    protected string name;
    protected string desc;

    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            name = value;
        }
    }

    public string Desc
    {
        get
        {
            return this.desc;
        }
        set
        {
            desc = value;
        }
    }

    /// <summary>
    /// A function that applies to specific stat increases for this item
    /// </summary>
    /// <param name="player">The player to apply the stat increases to</param>
    abstract public void applyBuffs(Player player);
    /// <summary>
    /// A function to reverse the specific stat increases for this item. Used when the item is dequipped.
    /// </summary>
    /// <param name="player">The player to reverse the stat increases of</param>
    abstract public void revertBuffs(Player player);
}

//ADDED 7 more items for Assment 3

/// <summary>
/// An item that increases a player's attack stat by 5
/// </summary>
public class Hammer : Item
{

    public Hammer()
    {
        this.name = "Hammer";
        this.desc = "Increases user's attack power by 5";
    }

    //Increase user's attack by 5
    public override void applyBuffs(Player player)
    {
        player.Attack += 5;
    }

    public override void revertBuffs(Player player)
    {
        player.Attack -= 5;
    }

}
/// <summary>
/// An item that increases a player's attack stat by 10
/// </summary>
public class Sword : Item
{

    public Sword()
    {
        this.name = "Sword";
        this.desc = "Increases user's attack power by 10";
    }

    //Increase user's attack by 5
    public override void applyBuffs(Player player)
    {
        player.Attack += 10;
    }

    public override void revertBuffs(Player player)
    {
        player.Attack -= 10;
    }

}

/// <summary>
/// An item that increases a player's attack stat by 25
/// </summary>
public class MagicSword : Item
{

    public MagicSword()
    {
        this.name = "Magic Sword";
        this.desc = "Increases user's attack power by 25";
    }

    //Increase user's attack by 5
    public override void applyBuffs(Player player)
    {
        player.Attack += 25;
    }

    public override void revertBuffs(Player player)
    {
        player.Attack -= 25;
    }

}

/// <summary>
/// An item that increases a player's speed stat by 5
/// </summary>
public class Trainers : Item
{

    public Trainers()
    {
        this.name = "Trainers";
        this.desc = "Increases user's speed by 5";
    }

    public override void applyBuffs(Player player)
    {
        player.Speed += 5;
    }

    public override void revertBuffs(Player player)
    {
        player.Speed -= 5;
    }

}
/// <summary>
/// An item that increases a player's speed stat by 10
/// </summary>
public class NikesPremiumShoes : Item
{

    public NikesPremiumShoes()
    {
        this.name = "Nikes Premium Shoes";
        this.desc = "Increases user's speed by 10";
    }

    public override void applyBuffs(Player player)
    {
        player.Speed += 10;
    }

    public override void revertBuffs(Player player)
    {
        player.Speed -= 10;
    }

}
/// <summary>
/// An item that increases a player's luck stat by 5
/// </summary>
public class RabbitFoot : Item
{

    public RabbitFoot()
    {
        this.name = "Rabbit Foot";
        this.desc = "Increases user's luck by 5";
    }

    public override void applyBuffs(Player player)
    {
        player.Luck += 5;
    }

    public override void revertBuffs(Player player)
    {
        player.Luck -= 5;
    }

}
/// <summary>
/// An item that increases a player's luck stat by 10
/// </summary>
public class RabbitCarcass : Item
{

    public RabbitCarcass()
    {
        this.name = "Rabbit Carcass";
        this.desc = "Increases user's luck by 10";
    }

    public override void applyBuffs(Player player)
    {
        player.Luck += 10;
    }

    public override void revertBuffs(Player player)
    {
        player.Luck -= 10;
    }

}
/// <summary>
/// An item that increases a player's maximum magic points by 3
/// </summary>
public class MagicAmulet : Item
{

    public MagicAmulet()
    {
        this.name = "Magic Amulet";
        this.desc = "Increases user's maximum magic points by 3";
    }

    public override void applyBuffs(Player player)
    {
        player.MaximumMagic += 3;
    }

    public override void revertBuffs(Player player)
    {
        player.MaximumMagic -= 3;
        if (player.Magic > player.MaximumMagic)
        {
            player.Magic = player.MaximumMagic;
        }
    }

}
/// <summary>
/// An item that increases a player's maximum magic points by 6
/// </summary>
public class MagicHat : Item
{

    public MagicHat()
    {
        this.name = "Magic Hat";
        this.desc = "Increases user's maximum magic points by 6";
    }

    public override void applyBuffs(Player player)
    {
        player.MaximumMagic += 6;
        player.Magic += 6;
    }

    public override void revertBuffs(Player player)
    {
        player.MaximumMagic -= 6;
        if (player.Magic > player.MaximumMagic)
        {
            player.Magic = player.MaximumMagic;
        }
    }

}
/// <summary>
/// An item that increases a player's defence stat by 5
/// </summary>
public class Shield : Item
{

    public Shield()
    {
        this.name = "Shield";
        this.desc = "Increases user's defence by 5";
    }

    public override void applyBuffs(Player player)
    {
        player.Defence += 5;
    }

    public override void revertBuffs(Player player)
    {
        player.Defence -= 5;
    }

}

/// <summary>
/// An item that increases a player's defence stat by 10
/// </summary>
public class DiamondShield : Item
{

    public DiamondShield()
    {
        this.name = "Diamond Shield";
        this.desc = "Increases user's defence by 10";
    }

    public override void applyBuffs(Player player)
    {
        player.Defence += 10;
    }

    public override void revertBuffs(Player player)
    {
        player.Defence -= 10;
    }

}

/// <summary>
/// An item that increases a player's attack stat by 2 and defence stat by 3
/// </summary>
public class Armour : Item
{

    public Armour()
    {
        this.name = "Armour";
        this.desc = "Increase attack by 2 and defence by 3";
    }

    public override void applyBuffs(Player player)
    {
        player.Attack += 3;
        player.Defence += 3;
    }

    public override void revertBuffs(Player player)
    {
        player.Attack -= 3;
        player.Defence -= 3;
    }

}
/// <summary>
/// An item that increases a player's attack stat by 5 and defence stat by 7
/// </summary>
public class GoldenArmour : Item
{

    public GoldenArmour()
    {
        this.name = "Golden Armour";
        this.desc = "Increase attack by 5 and defence by 7";
    }

    public override void applyBuffs(Player player)
    {
        player.Attack += 5;
        player.Defence += 7;
    }

    public override void revertBuffs(Player player)
    {
        player.Attack -= 5;
        player.Defence -= 7;
    }

}
