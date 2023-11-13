using System.Numerics;
using System.Security.Cryptography;
using System.Threading;

namespace TextRPG
{
    public class Creature
    {
        // 이름, 체력, 레벨, 경험치, 공격력, 방어력, 장착중인 무기, 장착중인 갑옷
        public string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int EXP { get; set; }
        public int AttackDamage { get; set; }
        public int Defense { get; set; }
        public int Gold { get; set; } 
        // 장착중인 무기
        // 장착중인 갑옷
        
        // 공격 및 피해 로직처리
    }

    public class Player : Creature
    {
        // 인벤토리 기능 리스트
        public List<Item> inventory;

        public string isWearingSwrod = "";
        public string isWearingArmor = "";

        // Player생성자
        public Player(string name, int health, int level, int exp, int attackDamage, int defense, int gold)
        {
            this.Name = name;
            this.Health = health;
            this.Level = level;
            this.EXP = exp;
            this.AttackDamage = attackDamage;
            this.Defense = defense;
            this.Gold = gold;
            inventory = new List<Item>();

            //isWearingSwrod = null;
            //isWearingArmor = null;
        }

        /*
        // Player Inventory생성자
        public Player()
        {
            inventory = new List<Item>();
        }
        */

        // player inventory에 item을 추가하는 기능 ( 상점에서 구매 )
        public void InsertItem(Player player, Item item)
        {
            if(player.Gold >= item.Price)
            {
                
                inventory.Add(item);    // Player 인벤토리에 해당 아이템 추가.
                player.Gold -= item.Price;  // 아이템 가격만큼 돈 차감
                Console.WriteLine($"{item.Name}을 구매했하였습니다.     현재 Gold : {player.Gold}");
                Thread.Sleep(1000);
            }
            else if(player.Gold < item.Price)
            {
                Console.WriteLine($"보유한 Gold가 부족합니다.     현재 Gold : {player.Gold}");
                Thread.Sleep(1000);
            }
        }

        // player inventory에 item을 빼는 기능 ( 상점에서 판매 )
        public void ReleaseItem(Item item)
        {
            inventory.Remove(item);
        }

        // item 장착 메서드
        public void EquipItem(Player player, int number)
        {
            // 검을 착용한 상태가 아니고, 아이템 타입이 검일때
            if (player.isWearingSwrod == "" && player.inventory[number].equipments == Item.Equipments.Swrod)
            {
                player.AttackDamage += player.inventory[number].Effect; // 장비 수치만큼 플레이어 능력치를 더함
                player.isWearingSwrod = player.inventory[number].Name;  // 검을 착용한 상태로 바꿔줌.
            }
            else if(player.isWearingArmor == "" && player.inventory[number].equipments == Item.Equipments.Armor)
            {
                player.Defense += player.inventory[number].Effect; // 장비 수치만큼 플레이어 능력치를 더함
                player.isWearingArmor = player.inventory[number].Name;  // 검을 착용한 상태로 바꿔줌.
            }
            else if (player.inventory[number].equipments == Item.Equipments.Swrod)
            {
                player.AttackDamage -= player.inventory[number].Effect; // 장비 수치만큼 플레이어 능력치를 빼줌
                player.isWearingSwrod = "";  // 검을 해제한 상태로 바꿈
            }
            else if (player.inventory[number].equipments == Item.Equipments.Armor)
            {
                player.Defense -= player.inventory[number].Effect; // 장비 수치만큼 플레이어 능력치를 더함
                player.isWearingArmor = "";  // 검을 착용한 상태로 바꿔줌.
            }
            else Console.WriteLine("이미 착용된 아이템.");

        }

        // item 해제 메서드
        public void UnlockItem(Player player, Item item, int number)
        {

        }
    }

    public class Enemy
    {
        // 하급던전 몬스터
        public Monster Goblin { get; set; } // 고블린
        public Monster Ork { get; set; } // 오크

        // 중급던전 몬스터
        public Monster Gnome { get; set; } // 노움
        public Monster Gorlem { get; set; } // 골렘

        // 상급던전 몬스터
        public Monster DaekElf { get; set; } // 다크엘프
        public Monster Griffon {  get; set; } // 그리폰
        public Monster Dragon {  get; set; } // 드래곤

        // 지옥 몬스터
        public Monster Lucifer { get; set; } // 자만의 루시퍼
        public Monster Mamon { get; set; } // 탐욕의 마몬
        public Monster Asmodeus { get; set; } // 음란의 아스모데우스
        public Monster Beelzebub { get; set; } // 대식의 벨제브브
        public Monster Leviathan { get; set; } // 질투의 리바이어선
        public Monster Belphegor { get; set; } // 나태의 벨페고르
        public Monster Satan {  get; set; } // 분노의 사탄


        // Enemy 생성자
        public Enemy(Monster goblin, Monster ork)
        {
            this.Goblin = goblin;
            this.Ork = ork;
        }
    }

    public class GameManager
    {
        // public static GameManager manager = new GameManager();
        #region itemSpawner

        #endregion

        // 입력값 변수
        ConsoleKeyInfo e;
        
        // 번호 선택 변수
        public int choiceNumber {  get; set; }
        
        // Player그릇
        public Player player {  get; set; }
        // ItemManager 그릇
        public ItemManager itemManager { get; set; }

        // GameManager 생성자
        public GameManager()
        {
            // Player 객체 생성
            player = new Player("Juno", 100, 1, 0, 10, 0, 100000);
            // ItemManager 객체 생성
            itemManager = new ItemManager();
        }

        // 1. 타이틀 화면
        public void TitleScene()
        {
            // 화면 초기화
            Console.Clear();

            Console.WriteLine("================================================================================");
            Console.WriteLine("=                         C# Console Game RPG                                  =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=            C #                              R-o l e                          =");
            Console.WriteLine("=             C o n s o l e                     P-l a y i n g                  =");
            Console.WriteLine("=              G a m e                            G-a m e                      =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                       Main by - GreatJun     =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=========================    1 . 시작하기    ===================================");
            Console.WriteLine("=========================    2 . 이어하기    ===================================");
            Console.WriteLine("=========================    3 . 종료하기    ===================================");
            Console.WriteLine("================================================================================");

            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch (e.Key)
            {
                case ConsoleKey.D1:
                    StroyScene();
                    break;
                case ConsoleKey.D2:
                    // 미구현
                    Console.WriteLine("아직 미구현 입니다.");
                    Thread.Sleep(1500);
                    TitleScene();
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("종료합니다...");
                    break;
                default:
                    Console.WriteLine("올바른 수를 입력해주시오.");
                    TitleScene();
                    break;
            }
        }

        // 2. 스토리 및 상황설명 화면
        public void StroyScene()
        {
            // 화면 초기화
            Console.Clear();

            Console.WriteLine("당신은 어떤 숲에서 정신을 차렸다...");
            Console.WriteLine("주변에 있는 것이라고는 나무와 식물들뿐....");
            Console.WriteLine("그렇게 정신을 차리던 도중 저 멀리 마을로 예상되는 빛이 보이기 시작했고");
            Console.WriteLine("그와 동시에 멀지 않은 곳에서 비릿한 피 냄새가 내 코를 찌르기 시작했다...");
            Console.WriteLine("");
            Console.WriteLine("1. 마을로 가본다.");
            Console.WriteLine("2. 피 냄새가 나는 곳으로 가본다.");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch (e.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    Console.WriteLine("뒤에서 들려오는 정체모를 소리를 뒤로하고 마을로 달렸다..."); 
                    Thread.Sleep(3000);
                    MainScene();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    Console.WriteLine("아뿔싸! 기억이 났다.....");
                    Console.WriteLine("나는 드래곤 원정을 하던 도중이었고 승산이 없다고 느껴져");
                    Console.WriteLine("도망을 가던 도중이었다... 내 앞에는 엄청난 크기의 드래곤이");
                    Console.WriteLine("장엄한 모습으로 서있다... 나는 이제 끝이다... ");
                    Console.WriteLine("======================= End =======================");
                    Thread.Sleep(8000);
                    TitleScene();
                    break;
                default:
                    Console.WriteLine("올바른 수를 입력해주십시오.");
                    Thread.Sleep(1000);
                    StroyScene();
                    break;
            }

        }

        // 3. 메인화면
        public void MainScene()
        {
            //화면 초기화
            Console.Clear();

            Console.WriteLine("================================================================================");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=      :시골의 작은 마을이다. 활기가 넘치고 여기저기서 웃음소리가 들린다.:     =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("=                            어떤일을 할까?                                    =");
            Console.WriteLine("=                                                                              =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=========================    1 . 상태창      ===================================");
            Console.WriteLine("=========================    2 . 인벤토리    ===================================");
            Console.WriteLine("=========================    3 . 상점        ===================================");
            Console.WriteLine("=========================    4 . 던전        ===================================");
            Console.WriteLine("=========================    5 . 타이틀 화면 ===================================");
            Console.WriteLine("================================================================================");

            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch (e.Key) 
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    Statistics();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    Inventory();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Shop();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    InDungeon();
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    TitleScene();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("올바른 수를 입력해주십시오.");
                    Thread.Sleep(1000);
                    MainScene();
                    break;
            }
        }

        // 상태창
        public void Statistics()
        {
            // 화면 초기화
            Console.Clear();

            Console.WriteLine("================================================================================");
            Console.WriteLine("                       이름 : {0}                                               ", player.Name);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       체력 : {0}                                               ", player.Health);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       레벨 : {0}                                               ", player.Level);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       경험치 : {0}                                             ", player.EXP);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       공격력 : {0}                                      ", player.AttackDamage);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       방어력 : {0} +                                  ", player.Defense);
            Console.WriteLine("================================================================================");
            Console.WriteLine("=            메인화면으로 돌아가려면 1번을 입력해주세요.                       =");
            Console.WriteLine("================================================================================");

            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch(e.Key)
            {
                case ConsoleKey.D1:
                    MainScene();
                    break;
                default:
                    Console.WriteLine("올바른 수를 입력하시오.");
                    Thread.Sleep(1000);
                    Statistics();
                    break;
            }
        }

        // 던전
        public void InDungeon()
        {
            // 화면 초기화
            Console.Clear();

            Console.WriteLine("================================================================================");
            Console.WriteLine("=                       1. 하급던전                                            =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                       2. 중급던전                                            =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                       3. 상급던전                                            =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                       4.   지옥                                              =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                       5. 메인화면                                            =");
            Console.WriteLine("================================================================================");

            Console.WriteLine(" ");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch (e.Key)
            {
                case ConsoleKey.D1:
                    Program.LowClass(player);
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    MainScene();
                    break;
                default:
                    break;
            }
        }

        // 인벤토리
        public void Inventory()
        {
            // 화면 초기화
            Console.Clear();

            while (true)
            {
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("     이름     ㅣ     효과     ㅣ     가격     ㅣ     설명     ");
                Console.WriteLine("");
                string isEquipment = "";
                for (int i = 0; i < player.inventory.Count; i++)
                {
                    if (player.isWearingSwrod == player.inventory[i].Name || player.isWearingArmor == player.inventory[i].Name) isEquipment = "[E]";
                    else isEquipment = "";
                    Console.WriteLine("{0} . {1} {2}     l{3}     l{4}     l{5}", i, isEquipment, player.inventory[i].Name, player.inventory[i].Effect, player.inventory[i].Price, player.inventory[i].Explanation);
                }

                Console.WriteLine("");
                Console.WriteLine("================================================================================");
                Console.WriteLine("                        착용할 아이템을 골라주십시오.                           ");
                Console.WriteLine("================================================================================");
                Console.WriteLine("                    뒤로 돌아가려면 Enter를 눌러주십시오.                       ");
                Console.WriteLine("================================================================================");
                Console.WriteLine("");
                Console.Write(">> ");

                e = Console.ReadKey();
                switch (e.Key)
                {
                    case ConsoleKey.D0:
                        player.EquipItem(player, 0);
                        break;
                    case ConsoleKey.D1:
                        player.EquipItem(player, 1);
                        break;
                    case ConsoleKey.D2:
                        player.EquipItem(player, 2);
                        break;
                    case ConsoleKey.D3:
                        player.EquipItem(player, 3);
                        break;
                    case ConsoleKey.D4:
                        player.EquipItem(player, 4);
                        break;
                    case ConsoleKey.D5:
                        player.EquipItem(player, 5);
                        break;
                    case ConsoleKey.D6:
                        player.EquipItem(player, 6);
                        break;
                    case ConsoleKey.D7:
                        player.EquipItem(player, 7);
                        break;
                    case ConsoleKey.D8:
                        player.EquipItem(player, 8);
                        break;
                    case ConsoleKey.Enter:
                        MainScene();
                        break;
                    default:
                        break;
                }
                Console.Clear();
            }
        }

        // 상점 입구
        public void Shop()
        {
            // 화면 초기화
            Console.Clear();

            Console.WriteLine("여기 없는건 어딜 가도 구할 수 없습니다~");
            Console.WriteLine("일단 들어와 보시라~ 와서 구경이라도 하고 가십쇼!");
            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구입");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("3. 메인화면 이동");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();

            switch (e.Key)
            {
                case ConsoleKey.D1:
                    BuyShop();
                    break;
                case ConsoleKey.D2:
                    SellShop();
                    break;
                case ConsoleKey.D3:
                    MainScene();
                    break;
                default:
                    Console.WriteLine("올바른 수를 입력하시오.");
                    Thread.Sleep(1000);
                    Shop();
                    break;
            }

        }

        // 구입 상점
        public void BuyShop()
        {
            // 화면 초가화
            Console.Clear();

            Console.WriteLine("[상점 목록]");
            Console.WriteLine("     이름     ㅣ     효과     ㅣ     가격     ㅣ     설명     ");
            Console.WriteLine("");

            for(int i = 0; i < itemManager.items.Count; i++)
            {
                Console.WriteLine("{0} . {1}     l{2}     l{3}     ㅣ{4}", i, itemManager.items[i].Name, itemManager.items[i].Effect, itemManager.items[i].Price, itemManager.items[i].Explanation);   // 아이템 이름, 효과, 가격, 설명
            }

            Console.WriteLine("================================================================================");
            Console.WriteLine("                    뒤로 돌아가려면 Enter를 눌러주십시오.                       ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("구입하고 싶은 아이템의 번호를 입력해주세요.");
            Console.Write(">> ");

            while (true)
            {
                e = Console.ReadKey();
                switch (e.Key)
                {
                    case ConsoleKey.D0:
                        this.player.InsertItem(player, itemManager.items[0]);   // Player클래스 아이템 구입 메서드
                        break;
                    case ConsoleKey.D1:
                        this.player.InsertItem(player, itemManager.items[1]);
                        break;
                    case ConsoleKey.D2:
                        this.player.InsertItem(player, itemManager.items[2]);
                        break;
                    case ConsoleKey.D3:
                        this.player.InsertItem(player, itemManager.items[3]);
                        break;
                    case ConsoleKey.D4:
                        this.player.InsertItem(player, itemManager.items[4]);
                        break;
                    case ConsoleKey.D5:
                        this.player.InsertItem(player, itemManager.items[5]);
                        break;
                    case ConsoleKey.D6:
                        this.player.InsertItem(player, itemManager.items[6]);
                        break;
                    case ConsoleKey.D7:
                        this.player.InsertItem(player, itemManager.items[7]);
                        break;
                    case ConsoleKey.Enter:
                        Shop();
                        break;
                    default:
                        Console.WriteLine("올바른 숫자를 입력하시오.");
                        break;
                }
            }

        }
        
        // 판매 상점
        public void SellShop()
        {

        }
    }

    public class ItemManager
    {
        public List<Item> items;

        // ItemManager 생성자
        public ItemManager()
        {
            this.items = new List<Item>();  // Item구조체 타입으로 items리스트 생성
            // 무기 종류
            Item longSwrod = new Item("롱소드", 30, 100, "철로 만들어진 평범한 양산형 롱소드이다.", Item.Equipments.Swrod);
            Item bastardSwrod = new Item("바스타드 소드", 100, 500, "검이 짧지만 불순물이 적어 좋은 광석으로 만들어진 검이다.", Item.Equipments.Swrod);
            Item zweihander = new Item("츠바이핸더", 500, 5000, "왕국 정예 기사들이 사용하는 검이다.", Item.Equipments.Swrod);
            Item claymore = new Item("클레이모어", 2000, 55000, "상급 익스퍼트 이상의 기사에게만 주어지는 검이다.", Item.Equipments.Swrod);
            // 방어구 종류
            Item leatherArmor = new Item("가죽 갑옷", 10, 200, "들짐승들의 가죽으로 만들어진 조잡한 가죽 갑옷이다.", Item.Equipments.Armor);
            Item ironArmor = new Item("철 갑옷", 40, 1000, "좋지도 나쁘지도 않은 평범한 철 갑옷이다.", Item.Equipments.Armor);
            Item mithrilArmor = new Item("미스릴 갑옷", 200, 10000, "청록색 빛이 나는 튼튼해 보이는 갑옷이다. 왕국 정예 기사들이 사용한다.", Item.Equipments.Armor);
            Item amantadiumArmor = new Item("아만타디움 갑옷", 1000, 120000, "단장급 이상에게만 주어지는 눈부시도록 하얀 백금색 갑옷이다.", Item.Equipments.Armor);
            // 아이템 등록
            // 무기
            items.Add(longSwrod); // 0
            items.Add(bastardSwrod);  // 1
            items.Add(zweihander); // 2
            items.Add(claymore); // 3
            // 방어구
            items.Add(leatherArmor); // 4
            items.Add(ironArmor); // 5
            items.Add(mithrilArmor); // 6
            items.Add(amantadiumArmor); // 7
        }
    }

    // Monster생성기
    public struct Monster
    {
        // Monster 이름, 체력, 공격력, 방어력, 설명
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackDamage { get; set; }
        public int Defense { get; set; }

        // Monster 생성자
        public Monster(string name, int health, int attackDamage, int defense)
        {
            this.Name = name;
            this.Health = health;
            this.AttackDamage = attackDamage;
            this.Defense = defense;
        }
    }

    // Item생성기
    public class Item
    {
        // 장비의 종류
        public enum Equipments
        {
            Swrod,
            Armor
        }
        public Equipments equipments; 

        // Item 이름, 공격력 or 방어력, 가격, 설명, 착용여부
        public string Name { get; set; }
        public int Effect { get; set; }
        public int Price { get; set; }
        public string Explanation { get; set; }

        // Item 생성자
        public Item(string name, int attackDamage, int price, string explanation, Equipments equipments)
        {
            this.Name = name;
            this.Effect = attackDamage;
            this.Price = price;
            this.Explanation = explanation;
            this.equipments = equipments;;
        }
    }

    internal class Program
    {
        // 키 입력값
        static ConsoleKeyInfo e;

        public static Program main = new Program();

        public static GameManager _gameManager;

        // 선택번호 변수
        static public int choiceNumber = 0;

        static void Main(string[] args)
        {
            // Monster 객체 생성
            Monster Goblin = new Monster("Goblin", 30, 5, 0);   // Goblin Object
            Monster Ork = new Monster("Ork", 300, 20, 10);    // Ork Object

            // Enemy 객체 생성
            Enemy enemy = new Enemy(Goblin, Ork);

            // GameManager 객체 생성
            _gameManager = new GameManager();

            // 게임 시작
            // 시작화면 호출
            _gameManager.TitleScene();
        }

        // 던전 종류
        static public void LowClass(Player player)
        {
            // 웨이브 수
            int wave = 5;
            
            // 화면 초기화
            Console.Clear();

            // 몬스터,플레이어 죽음 체크
            bool isMonster = true;
            bool isPlayer = true;

            // 하급던전 이동 로딩
            Console.WriteLine("하급 던전으로 이동중.....");
            Thread.Sleep(3000);

            // Wave동안 반복
            for (int i = 0; i < wave; i++)
            {
                // 화면 초기화
                Console.Clear();

                // 몬스터 생성
                Monster Goblin = new Monster("Goblin", 30, 6, 0);   // Goblin Object

                // 몬스터 생명 초기화
                isMonster = true;

                // 적 조우
                Console.WriteLine("{0}을 만났다 !", Goblin.Name);
                Console.WriteLine("Wave : {0} / 5 ", i);
                while (isMonster && isPlayer)
                {
                    Console.WriteLine("===================================================");
                    Console.WriteLine("{0}의 체력 : {1}    {2}의 체력 : {3}", player.Name, player.Health, Goblin.Name, Goblin.Health);

                    Console.WriteLine("");
                    Console.WriteLine("1. 공격   2. 도망");
                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">> ");
                    e = Console.ReadKey();
                    switch (e.Key)
                    {
                        case ConsoleKey.D1:
                            Console.WriteLine("{0}에게 {1}의 데미지를 입혔다!", Goblin.Name, player.AttackDamage - Goblin.Defense);
                            Goblin.Health -= (player.AttackDamage - Goblin.Defense);
                            Thread.Sleep(100);
                            Console.WriteLine("");
                            Console.WriteLine("{0}이 {1}한테 {2}의 데미지를 입혔다...", Goblin.Name, player.Name, Goblin.AttackDamage - player.Defense);
                            player.Health -= (Goblin.AttackDamage - player.Defense);
                            Thread.Sleep(100);
                            Console.WriteLine("");
                            if (player.Health <= 0)
                            {
                                player.Health = 0;
                                Console.WriteLine("{0}이 사망하였습니다...", player.Name);
                                Thread.Sleep(3000);
                                isPlayer = false;
                                // 타이틀 화면으로 이동
                                _gameManager.TitleScene();
                            }
                            else if (Goblin.Health <= 0)
                            {
                                Goblin.Health = 0;
                                Console.WriteLine("{0}을 잡았습니다!", Goblin.Name);
                                player.Gold += 100;
                                Console.WriteLine("100Gold를 얻었습니다.");
                                Thread.Sleep(2000);
                                isMonster = false;
                                break;
                            }
                            else
                            {
                                Thread.Sleep(2000);
                                Console.Clear();
                            }
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Console.WriteLine("전속력으로 도망갔다.");
                            Thread.Sleep(1000);
                            // 타이틀 화면으로 이동
                            _gameManager.MainScene();
                            break;
                        default:
                            Console.WriteLine("올바른 수를 입력해주시오.");
                            break;
                    }
                }
            }

            // 하급던전 Boss 출현
            Monster Ork = new Monster("Ork", 50, 10, 4);  // 보스 생성
            // 몬스터 생명 초기화
            isMonster = true;

            // 화면 초기화
            Console.Clear();

            Console.WriteLine("===================================================");
            Console.WriteLine("===================================================");
            Console.WriteLine("===================================================");
            Console.WriteLine("하급던전의 주인 {0}와 마주쳤다...");
            while (isMonster && isPlayer)
            {
                Console.WriteLine("===================================================");
                Console.WriteLine("{0}의 체력 : {1}    {2}의 체력 : {3}", player.Name, player.Health, Ork.Name, Ork.Health);

                Console.WriteLine("");
                Console.WriteLine("1. 공격   2. 도망");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                e = Console.ReadKey();
                switch(e.Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("{0}에게 {1}의 데미지를 입혔다!", Ork.Name, player.AttackDamage - Ork.Defense);
                        Ork.Health -= (player.AttackDamage - Ork.Defense);
                        Thread.Sleep(100);
                        Console.WriteLine("");
                        Console.WriteLine("{0}이 {1}한테 {2}의 데미지를 입혔다...", Ork.Name, player.Name, Ork.AttackDamage - player.Defense);
                        player.Health -= (Ork.AttackDamage - player.Defense);
                        Thread.Sleep(100);
                        Console.WriteLine("");
                        if (player.Health <= 0)
                        {
                            player.Health = 0;
                            Console.WriteLine("{0}이 사망하였습니다...", player.Name);
                            Thread.Sleep(3000);
                            isPlayer = false;
                            // 타이틀 화면으로 이동
                            _gameManager.TitleScene();

                        }
                        else if (Ork.Health <= 0)
                        {
                            Ork.Health = 0;
                            Console.WriteLine("{0}을 잡았습니다!", Ork.Name);
                            player.Gold += 500;
                            Console.WriteLine("500Gold를 얻었습니다.");
                            Thread.Sleep(2000);
                            isMonster = false;
                            break;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                            Console.Clear();
                        }

                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine("전속력으로 도망갔다.");
                        Thread.Sleep(1000);
                        // 타이틀 화면으로 이동
                        _gameManager.MainScene();
                        break;
                    default:
                        Console.WriteLine("올바른 수를 입력해주시오.");
                        break;
                }
            }
        }

        public void MiddleClass()
        {

        }

        public void HighClass()
        {

        }

        public void HellClass()
        {

        }
    }
}