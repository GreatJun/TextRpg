using System.ComponentModel;
using System.IO.Enumeration;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TextRPG
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int EXP { get; set; }
        public int AttackDamage {  get; set; }
        public int Defense {  get; set; }
        public int Gold { get; set; }

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

            // 초기지급 아이템
            Item startSwrod = new Item("낡은 검", 5, 0, "여기저기 금이 가고 낡은 검이다.", Item.Equipments.Swrod);
            Item startArmor = new Item("거적때기 옷", 1, 0, "냄새나고 낡은 옷이다", Item.Equipments.Armor);
            inventory.Add(startSwrod);
            inventory.Add(startArmor);
        }

        // player inventory에 item을 추가하는 기능 ( 상점에서 구매 )
        public void InsertItem(Item item)
        {
            if(this.Gold >= item.Price)
            {
                
                inventory.Add(item);    // Player 인벤토리에 해당 아이템 추가.
                this.Gold -= item.Price;  // 아이템 가격만큼 돈 차감
                Console.WriteLine("{0}을 구매했습니다.", item.Name);
                Thread.Sleep(1000);
            }
            else if(this.Gold < item.Price)
            {
                Console.WriteLine($"보유한 Gold가 부족합니다.     현재 Gold : {this.Gold}");
                Thread.Sleep(1000);
            }
        }

        // player inventory에 item을 빼는 기능 ( 상점에 판매 )
        public void ReleaseItem(int index)
        {
            if (this.inventory[index] != null)
            {
                inventory.Remove(this.inventory[index]);
                this.Gold += this.inventory[index].Price / 2;
            }
            else Console.WriteLine("아이템이 존재하지 않습니다.");
        }

        // item 장착 메서드
        public void EquipItem(int number)
        {
            Item item = this.inventory[number];

            if(item.equipments == Item.Equipments.Swrod)
            {
                this.AttackDamage = ToggleEquipmentState(ref isWearingSwrod, this.AttackDamage, item);
            }
            else if(item.equipments == Item.Equipments.Armor)
            {
                this.Defense = ToggleEquipmentState(ref isWearingArmor, this.Defense, item);
            }
        }

        private int ToggleEquipmentState(ref string isWearing, int stat, Item item)
        {
            if(isWearing == "")
            {
                stat += item.Effect;
                isWearing = item.Name;
            }
            else
            {
                stat -= item.Effect;
                isWearing = "";
            }
            return stat;
        }

        // 레벨업 메서드
        public void LevelUp()
        {
            // 레벨업시 체력10, 공격력5, 방어력1 증가
            this.Level++;
            this.Health += 10;
            this.AttackDamage += 5;
            this.Defense += 1;
        }

        // player 공격 메서드
        public int playerAttack(Monster enemy)
        {
            if (this.AttackDamage < enemy.Defense) return 0;    // 몬스터의 방어력이 플레이어의 공격력보다 높으면 0으로 변화없음
            else return this.AttackDamage - enemy.Defense;
        }
    }

    public class Enemy
    {
        #region Later
        /*
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

        시간이 부족해서 구현불가 입니다....
        */
        #endregion


        // Enemy 생성자
        public Enemy()
        {

        }

        // Enemy 공격 메서드
        public int enemyAttack(Player player, Monster monster)
        {
            if (player.Defense > monster.AttackDamage) return 0;
            else return monster.AttackDamage - player.Defense;
        }
    }

    public class GameManager
    {
        #region itemSpawner

        #endregion

        // 입력값 변수
        ConsoleKeyInfo e;
        
        // 번호 선택 변수
        public int choiceNumber {  get; set; }
        
        // Player그릇
        public static Player player {  get; set; }
        // ItemManager 그릇
        public ItemManager itemManager { get; set; }

        // GameManager 생성자
        public GameManager()
        {
            // Player객체 생성
            player = new Player("Juno", 100, 1, 0, 10, 0, 10000);
            // ItemManager 객체 생성
            itemManager = new ItemManager();
        }

        

        // 1. 타이틀 화면
        public void TitleScene()
        {
            string[] text = {"1 . 시작하기", "2 . 이어하기", "3 . 종료하기"};

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
            for(int i = 0; i < text.Length; i++)
            {
                Console.WriteLine($"=========================    {text[i]}    ===================================");
            }
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
                    // C:\JsonTest 경로 안에있는 save.json 파일을 string read에 넣는다.(파일 읽기)
                    string read = File.ReadAllText(@"C:\JsonTest\save.json");
                    // json문자열을 Player클래스 객체 player로 변환
                    player = JsonConvert.DeserializeObject<Player>(read);
                    Console.WriteLine("불러오는중 입니다.");
                    Thread.Sleep(1500);
                    MainScene();
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
            Console.WriteLine("=========================    6 . 저장하기    ===================================");
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
                case ConsoleKey.D6:
                    // Json
                    //player클래스를 Json 객체로 생성 ( List를 Json으로 변환 )
                    string save = JsonConvert.SerializeObject(player);
                    // Json 저장경로 및 저장
                    File.WriteAllText(@"C:\JsonTest\save.json", save.ToString());
                    Console.WriteLine("저장중 입니다...");
                    Thread.Sleep(1000);
                    MainScene();
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
            // 장착된 아이템의 공,방어력을 담아놀 변수
            int atk = 0;
            int def = 0;
            // 인벤토리에 장착된 아이템을 찾아 있으면 값을 넣어준다.
            foreach (Item item in player.inventory)
            {
                if (player.isWearingSwrod == item.Name) atk = item.Effect;
                else if (player.isWearingArmor == item.Name) def = item.Effect;

            }
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
            Console.WriteLine("                       공격력 : {0} + ({1})                                     ", player.AttackDamage, atk);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       방어력 : {0} + ({1})                                     ", player.Defense, def);
            Console.WriteLine("================================================================================");
            Console.WriteLine("                       Gold : {0}                                               ", player.Gold);
            Console.WriteLine("================================================================================");
            Console.WriteLine("=            메인화면으로 돌아가려면 Enter를 입력해주세요.                      =");
            Console.WriteLine("================================================================================");

            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            e = Console.ReadKey();
            switch(e.Key)
            {
                case ConsoleKey.Enter:
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
            Console.WriteLine("=                            1. 하급던전                                       =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                            2. 중급던전                                       =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                            3. 상급던전                                       =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                            4.   지옥                                         =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=                            5. 메인화면                                       =");
            Console.WriteLine("================================================================================");
            Console.WriteLine("=               체력은 던전에서 나오면 자동으로 회복됩니다.                    =");
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
                    Console.WriteLine("아직 미구현 입니다.");
                    Thread.Sleep(1500);
                    MainScene();
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("아직 미구현 입니다.");
                    Thread.Sleep(1500);
                    MainScene();
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("아직 미구현 입니다.");
                    Thread.Sleep(1500);
                    MainScene();
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

            ConsoleTable inventoryTable = new ConsoleTable("순서","이름", "효과", "가격", "설명");

            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("");
            string isEquipment = "";
            for (int i = 0; i < player.inventory.Count; i++)
            {
                if (player.isWearingSwrod == player.inventory[i].Name || player.isWearingArmor == player.inventory[i].Name) isEquipment = "[E]";
                else isEquipment = "";
                inventoryTable.AddRow($"{i}", $"{isEquipment} {player.inventory[i].Name}", $"{player.inventory[i].Effect}", $"{player.inventory[i].Price}", $"{player.inventory[i].Explanation}").Configure(o => o.EnableCount = false);
            }
            inventoryTable.Write();

            Console.WriteLine("");
            Console.WriteLine("================================================================================");
            Console.WriteLine("                        착용할 아이템을 골라주십시오.                           ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("                    뒤로 돌아가려면 Enter를 눌러주십시오.                       ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("");
            Console.Write(">> ");
            e = Console.ReadKey();

            int x = int.Parse(e.KeyChar.ToString());    // ConsoleKeyInfo값을 char형으로 변환 후 string으로 변환 후 int.Parse하여 정수로 변환한 값을 x에 대입.
            if (x > player.inventory.Count)
            {
                Console.WriteLine("올바른 수를 입력하시오.");
                Thread.Sleep(1000);
                Inventory();
                return;
            }
            switch (e.Key)
            {
                case ConsoleKey.D0:
                    player.EquipItem(0);
                    Inventory();
                    break;
                case ConsoleKey.D1:
                    player.EquipItem(1);
                    Inventory();
                    break;
                case ConsoleKey.D2:
                    player.EquipItem(2);
                    Inventory();
                    break;
                case ConsoleKey.D3:
                    player.EquipItem(3);
                    Inventory();
                    break;
                case ConsoleKey.D4:
                    player.EquipItem(4);
                    Inventory();
                    break;
                case ConsoleKey.D5:
                    player.EquipItem(5);
                    Inventory();
                    break;
                case ConsoleKey.D6:
                    player.EquipItem(6);
                    Inventory();
                    break;
                case ConsoleKey.D7:
                    player.EquipItem(7);
                    Inventory();
                    break;
                case ConsoleKey.D8:
                    player.EquipItem(8);
                    Inventory();
                    break;
                case ConsoleKey.DownArrow:
                    // OrderBy를 이용한 이름 내림차순 정렬
                    player.inventory = player.inventory.OrderBy(item => item.Name).ToList();
                    Inventory();
                    break;
                case ConsoleKey.UpArrow:
                    // OrderByDescending를 이용한 이름 오름차순 정렬
                    player.inventory = player.inventory.OrderByDescending(item => item.Name).ToList();
                    Inventory();
                    break;
                case ConsoleKey.RightArrow:
                    // LINQ를 이용한 아이템 능력치 내림차순 정렬
                    player.inventory = (from item in player.inventory
                                        orderby item.Effect
                                        select item).ToList();
                    Inventory();
                    break;
                case ConsoleKey.LeftArrow:
                    // LINQ를 이용한 아이템 장비종류(Enum)(Swrod, Armor) 오름차순 정렬
                    player.inventory = (from item in player.inventory
                                        orderby item.equipments descending
                                        select item).ToList();
                    Inventory();
                    break;
                case ConsoleKey.Enter:
                    MainScene();
                    break;
                default:
                    break;
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

            ConsoleTable buyShopTable = new ConsoleTable("순서", "이름", "효과", "가격", "설명");

            Console.WriteLine("[상점 목록]");
            Console.WriteLine("현재 소지Gold : {0}", player.Gold);
            Console.WriteLine("");

            for(int i = 0; i < itemManager.items.Count; i++)
            {
                buyShopTable.AddRow($"{i}", $"{itemManager.items[i].Name}", $"{itemManager.items[i].Effect}", $"{itemManager.items[i].Price}", $"{itemManager.items[i].Explanation}").Configure(o => o.EnableCount = false);   // 아이템 이름, 효과, 가격, 설명
            }
            buyShopTable.Write();
            Console.WriteLine("================================================================================");
            Console.WriteLine("                    뒤로 돌아가려면 Enter를 눌러주십시오.                       ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("");
            Console.WriteLine("구입하고 싶은 아이템의 번호를 입력해주세요.");
            Console.Write(">> ");

            while (true)
            {
                e = Console.ReadKey();
                Console.WriteLine("");
                switch (e.Key)
                {
                    case ConsoleKey.D0:
                        player.InsertItem(itemManager.items[0]);   // Player클래스 아이템 구입 메서드
                        BuyShop();
                        break;
                    case ConsoleKey.D1:
                        player.InsertItem(itemManager.items[1]);
                        BuyShop();
                        break;
                    case ConsoleKey.D2:
                        player.InsertItem(itemManager.items[2]);
                        BuyShop();
                        break;
                    case ConsoleKey.D3:
                        player.InsertItem(itemManager.items[3]);
                        BuyShop();
                        break;
                    case ConsoleKey.D4:
                        player.InsertItem(itemManager.items[4]);
                        BuyShop();
                        break;
                    case ConsoleKey.D5:
                        player.InsertItem(itemManager.items[5]);
                        BuyShop();
                        break;
                    case ConsoleKey.D6:
                        player.InsertItem(itemManager.items[6]);
                        BuyShop();
                        break;
                    case ConsoleKey.D7:
                        player.InsertItem(itemManager.items[7]);
                        BuyShop();
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
            // 화면 초기화
            Console.Clear();

            ConsoleTable shellShop = new ConsoleTable("순서", "이름", "효과", "가격", "설명");


            Console.WriteLine("[내 아이템 목록]");
            Console.WriteLine("현재 소지Gold : {0}", player.Gold);
            Console.WriteLine("");
            for (int i = 0; i < player.inventory.Count; i++)
            {
                shellShop.AddRow($"{i}", $"{player.inventory[i].Name}", $"{player.inventory[i].Effect}", $"{player.inventory[i].Price}", $"{player.inventory[i].Explanation}").Configure(o => o.EnableCount = false);
            }

            shellShop.Write();
            Console.WriteLine("================================================================================");
            Console.WriteLine("                    뒤로 돌아가려면 Enter를 눌러주십시오.                       ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("                  판매를 할 때는 반값밖에 돌려받지 못합니다.                    ");
            Console.WriteLine("================================================================================");
            Console.WriteLine("");
            Console.WriteLine("판매하고 싶은 아이템의 번호를 입력해주세요.");
            Console.Write(">> ");

            e = Console.ReadKey();
            Console.WriteLine("");
            switch (e.Key)
            {
                case ConsoleKey.D0:
                    player.ReleaseItem(0);   // Player클래스 아이템 구입 메서드
                    SellShop();
                    break;
                case ConsoleKey.D1:
                    player.ReleaseItem(1);
                    SellShop();
                    break;
                case ConsoleKey.D2:
                    player.ReleaseItem(2);
                    SellShop();
                    break;
                case ConsoleKey.D3:
                    player.ReleaseItem(3);
                    SellShop();
                    break;
                case ConsoleKey.D4:
                    player.ReleaseItem(4);
                    SellShop();
                    break;
                case ConsoleKey.D5:
                    player.ReleaseItem(5);
                    SellShop();
                    break;
                case ConsoleKey.D6:
                    player.ReleaseItem(6);
                    SellShop();
                    break;
                case ConsoleKey.D7:
                    player.ReleaseItem(7);
                    SellShop();
                    break;
                case ConsoleKey.Enter:
                    Shop();
                    break;
                default:
                    Console.WriteLine("올바른 숫자를 입력하시오.");
                    break;
            }
            Console.Clear();
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

        public static Enemy _enemy;


        // 선택번호 변수
        static public int choiceNumber = 0;

        static void Main(string[] args)
        {
            // GameManager 객체 생성
            _gameManager = new GameManager();



            // Enemy 객체 생성
            _enemy = new Enemy();

            // 게임 시작
            // 시작화면 호출
            _gameManager.TitleScene();
        }

        // 던전 종류
        static public void LowClass(Player player)
        {
            // player초기 체력 저장
            int firstHealth = player.Health;

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
                            Console.WriteLine("{0}에게 {1}의 데미지를 입혔다!", Goblin.Name, (player.AttackDamage - Goblin.Defense) < 0 ? 0 : (player.AttackDamage - Goblin.Defense));
                            Goblin.Health -= player.playerAttack(Goblin);   // player 공격 메서드
 
                            Thread.Sleep(100);
                            Console.WriteLine("");
                            Console.WriteLine("{0}이 {1}한테 {2}의 데미지를 입혔다...", Goblin.Name, player.Name, (Goblin.AttackDamage - player.Defense) < 0 ? 0 : (Goblin.AttackDamage - player.Defense));
                            player.Health -= _enemy.enemyAttack(player, Goblin);    // Enemy 공격 메서드

                            Thread.Sleep(100);
                            Console.WriteLine("");
                            if (player.Health <= 0)
                            {
                                player.Health = 0;
                                Console.WriteLine("{0}이 사망하였습니다...", player.Name);
                                Thread.Sleep(3000);
                                // 죽음 체크
                                isPlayer = false;
                                // 플레이어 체력 회복
                                player.Health = firstHealth;
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
                            // 플레이어 체력 회복
                            player.Health = firstHealth;
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
            Console.WriteLine("하급던전의 주인 {0}와 마주쳤다...", Ork.Name);
            Console.WriteLine("===================================================");
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
                        Console.WriteLine("{0}에게 {1}의 데미지를 입혔다!", Ork.Name, (player.AttackDamage - Ork.Defense) < 0 ? 0 : (player.AttackDamage - Ork.Defense));
                        Ork.Health -= player.playerAttack(Ork);   // player 공격 메서드

                        Thread.Sleep(100);
                        Console.WriteLine("");
                        Console.WriteLine("{0}이 {1}한테 {2}의 데미지를 입혔다...", Ork.Name, player.Name, (Ork.AttackDamage - player.Defense) < 0 ? 0 : Ork.AttackDamage - player.Defense);
                        player.Health -= _enemy.enemyAttack(player, Ork);    // Enemy 공격 메서드

                        Thread.Sleep(100);
                        Console.WriteLine("");
                        if (player.Health <= 0)
                        {
                            player.Health = 0;
                            Console.WriteLine("{0}이 사망하였습니다...", player.Name);
                            Thread.Sleep(3000);
                            isPlayer = false;
                            // 플레이어 체력 회복
                            player.Health = firstHealth;
                            // 타이틀 화면으로 이동
                            _gameManager.TitleScene();

                        }
                        else if (Ork.Health <= 0)
                        {
                            Ork.Health = 0;
                            Console.WriteLine("{0}을 잡았습니다!", Ork.Name);
                            player.Gold += 500;
                            Console.WriteLine("500Gold를 얻었습니다.");
                            Console.WriteLine("");
                            // 플레이어 체력 회복
                            player.Health = firstHealth;
                            // Player Level Up
                            player.LevelUp();
                            Console.WriteLine("Level UP !!!   Level : {0} -> Level : {1} ", player.Level - 1, player.Level);
                            Thread.Sleep(2000);
                            isMonster = false;
                            _gameManager.MainScene();
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
                        // 플레이어 체력 회복
                        player.Health = firstHealth;
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