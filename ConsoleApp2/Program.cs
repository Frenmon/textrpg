using System;
using System.Collections.Generic;

internal class Program
{
    private static Character player;
    private static List<Item> inventory;

    static void Main(string[] args)
    {
        GameDataSetting();
        DisplayGameIntro();
    }

    static void GameDataSetting()
    {
        player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

        inventory = new List<Item>
        {
            new Item("무쇠갑옷", "방어력 +5", true),
            new Item("낡은 검", "공격력 +2", false)
        };
    }

    static void DisplayGameIntro()
    {
        Console.Clear();

        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int input = CheckValidInput(1, 2);
        switch (input)
        {
            case 1:
                DisplayMyInfo();
                break;

            case 2:
                DisplayInventory();
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv. {player.Level}");
        Console.WriteLine($"{player.Name} ({player.Job})");
        Console.WriteLine($"공격력: {player.Atk} (+{player.GetEquipmentAttack()})");
        Console.WriteLine($"방어력: {player.Def} (+{player.GetEquipmentDefense()})");
        Console.WriteLine($"체력: {player.Hp}");
        Console.WriteLine($"Gold: {player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;
        }
    }

    static void DisplayInventory()
    {
        Console.Clear();

        Console.WriteLine("인벤토리 - 보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < inventory.Count; i++)
        {
            string equipped = inventory[i].IsEquipped ? "[E]" : "   ";
            Console.WriteLine($"{i + 1}. {equipped}{inventory[i].Name} | {inventory[i].Description}");
        }
        Console.WriteLine("\n0. 나가기");

        int input = CheckValidInput(0, inventory.Count);
        if (input == 0)
        {
            DisplayGameIntro();
        }
        else
        {
            ManageEquip(input - 1);
        }
    }

    static void ManageEquip(int itemIndex)
    {
        Item item = inventory[itemIndex];

        if (item.IsEquipped)
        {
            item.IsEquipped = false;
            Console.WriteLine($"{item.Name}을(를) 장착 해제했습니다.");
        }
        else
        {
            item.IsEquipped = true;
            Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
        }

        Console.WriteLine();
        Console.WriteLine("아무 키나 눌러주세요...");
        Console.ReadKey();
        DisplayInventory();
    }

    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    public static List<Item> GetInventory()
    {
        return inventory;
    }
}

public class Character
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; }
    public int Gold { get; }

    public Character(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
    }

    public int GetEquipmentAttack()
    {
        int equipmentAttack = 0;
        foreach (Item item in Program.GetInventory())
        {
            if (item.IsEquipped)
            {
                equipmentAttack += item.AttackBonus;
            }
        }
        return equipmentAttack;
    }

    public int GetEquipmentDefense()
    {
        int equipmentDefense = 0;
        foreach (Item item in Program.GetInventory())
        {
            if (item.IsEquipped)
            {
                equipmentDefense += item.DefenseBonus;
            }
        }
        return equipmentDefense;
    }
}

public class Item
{
    public string Name { get; }
    public string Description { get; }
    public int AttackBonus { get; }
    public int DefenseBonus { get; }
    public bool IsEquipped { get; set; }

    public Item(string name, string description, bool isEquipped)
    {
        Name = name;
        Description = description;
        AttackBonus = name.Contains("검") ? 2 : 0;
        DefenseBonus = name.Contains("갑옷") ? 5 : 0;
        IsEquipped = isEquipped;
    }
}
