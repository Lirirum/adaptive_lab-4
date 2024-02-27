using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

public class BankAccount
{
    // Поля класу
    protected internal string accountHolderName;
    private protected string accountNumber;
    private decimal balance;
    protected bool isActive;
    public DateTime lastTransactionDate;
    internal string message;
    static string error;

    // Конструктор класу
    public BankAccount(string name, string number, decimal initialBalance)
    {
        accountHolderName = name;
        accountNumber = number;
        balance = initialBalance;
        isActive = true;
        lastTransactionDate = DateTime.Now;
    }

    // Метод для зняття грошей з рахунку
    public bool Withdraw(decimal amount)
    {
        if (isActive)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"{amount} грошей знято з рахунку. Новий баланс: {balance}");
                lastTransactionDate = DateTime.Now;
                return true;
            }
            else
            {
                Console.WriteLine("Помилка: недостатньо коштів на рахунку або введено некоректну суму для зняття.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Помилка: рахунок неактивний. Зняття неможливе.");
            return false;
        }
    }

    // Метод для зарахування грошей на рахунок
    public bool Deposit(decimal amount)
    {
        if (isActive && amount > 0)
        {
            balance += amount;
            Console.WriteLine($"{amount} грошей зараховано на рахунок. Новий баланс: {balance}");
            lastTransactionDate = DateTime.Now;
            return true;
        }
        else
        {
            Console.WriteLine("Помилка: некоректна сума для зарахування або рахунок неактивний.");
            return false;
        }
    }

    // Метод для перевірки стану рахунку
    public decimal CheckBalance()
    {
        Console.WriteLine($"Назва власника рахунку: {accountHolderName}");
        Console.WriteLine($"Номер рахунку: {accountNumber}");
        Console.WriteLine($"Поточний баланс: {balance}");
        Console.WriteLine($"Дата останньої транзакції: {lastTransactionDate}");
        Console.WriteLine($"Статус рахунку: {(isActive ? "активний" : "неактивний")}");
        return balance;
    }

    // Метод для закриття рахунку
    public void CloseAccount()
    {
        if (balance == 0)
        {
            isActive = false;
            Console.WriteLine("Рахунок успішно закрито.");
        }
        else
        {
            Console.WriteLine("Помилка: на рахунку ще є кошти. Закриття неможливе.");
        }
    }


    // Метод для відновлення рахунку
    public void OpenAccount()
    {
        if (isActive == false)
        {
            isActive = true;
            Console.WriteLine("Рахунок успішно відновлено.");
        }
        else
        {
            Console.WriteLine("Помилка: рахунук вже активний.");
        }
    }

    public string GetMessage() { return message; }
    public void SetMessage(string message) {  this.message = message; }

    public void Message() {
        Console.WriteLine($"{accountHolderName}, для вас є повідомлення: {message}");
    }

}

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Type t1 = typeof(BankAccount);

            Console.WriteLine($"Name: {t1.Name}");              
            Console.WriteLine($"Full Name: {t1.FullName}");    
            Console.WriteLine($"Namespace: {t1.Namespace}");   
            Console.WriteLine($"Is struct: {t1.IsValueType}");  
            Console.WriteLine($"Is class: {t1.IsClass}");

            TypeInfo t1TypeInfo = t1.GetTypeInfo();

            Console.WriteLine($"Is Abstract: {t1TypeInfo.IsAbstract}");

            // Перевіряємо, чи є клас generic
            Console.WriteLine($"Is Generic: {t1TypeInfo.IsGenericType}");

            // Отримуємо всі оголошені методи класу
            MethodInfo[] declaredMethods = t1TypeInfo.DeclaredMethods.ToArray();

            // Виводимо інформацію про кожен метод
            Console.WriteLine("Declared Methods:");
            foreach (MethodInfo method in declaredMethods)
            {
                Console.WriteLine($"Method Name: {method.Name}, Return Type: {method.ReturnType}");
            }


            Console.WriteLine("\n//////////////////////////////////");
            foreach (MemberInfo member in t1.GetMembers())// За замовчуванням GetMembers буде давати доступа тільки до public елементів
            {
                Console.WriteLine($"{member.DeclaringType} {member.MemberType} {member.Name}");
            }
         
            Console.WriteLine("\n//////////////////////////////////");

            foreach (MemberInfo member in t1.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Console.WriteLine($"{member.DeclaringType} {member.MemberType} {member.Name}");
            }

            Console.WriteLine("\n//////////////////////////////////");

            MemberInfo[] method1 = t1.GetMember("Deposit", BindingFlags.Instance | BindingFlags.Public);
            foreach (MemberInfo member in method1)
            {
                Console.WriteLine($"{member.MemberType} {member.Name}");
            }



            foreach (MethodInfo method in t1.GetMethods())
            {


                string modificator = "";
                if (method.IsPublic)
                    modificator += "public ";
                else if (method.IsPrivate)
                    modificator += "private ";
                else if (method.IsAssembly)
                    modificator += "internal ";
                else if (method.IsFamily)
                    modificator += "protected ";
                else if (method.IsFamilyAndAssembly)
                    modificator += "private protected ";
                else if (method.IsFamilyOrAssembly)
                    modificator += "protected internal ";

                if (method.IsStatic) modificator += "static ";
    
                if (method.IsVirtual) modificator += "virtual ";

             
                Console.WriteLine($"{modificator}{method.ReturnType.Name} {method.Name} ()");
            }


            Console.WriteLine("\n//////////////////////////////////");
            Console.WriteLine("Поля:");
            foreach (FieldInfo field in t1.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
            {
                string modificator = "";


                switch (field.Attributes )
                {
                    case FieldAttributes.Public:
                        modificator = "public ";
                        break;
                    case FieldAttributes.Private:
                        modificator = "private ";
                        break;
                    case FieldAttributes.Assembly:
                        modificator = "internal ";
                        break;
                    case FieldAttributes.Family:
                        modificator = "protected ";
                        break;
                    case FieldAttributes.FamANDAssem:
                        modificator = "private protected ";
                        break;
                    case FieldAttributes.FamORAssem:
                        modificator = "protected internal ";
                        break;
                }
  
                if (field.IsStatic) modificator += "static ";

                Console.WriteLine($"{modificator}{field.FieldType.Name} {field.Name}");
            }


            Console.WriteLine("\n//////////////////////////////////");
            Console.WriteLine("Методи:");
            foreach (MethodInfo method in t1.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {

                Console.WriteLine($"{method.ReturnType.Name} {method.Name} ()");
            }
            Console.WriteLine("\n//////////////////////////////////");

            foreach (MethodInfo method in t1.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                ParameterInfo[] parameters = method.GetParameters();
                string paramInfo="";
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    paramInfo +=$"{param.ParameterType.Name}  {param.Name}";                  
                    if (param.HasDefaultValue) paramInfo=$"{paramInfo}={param.DefaultValue}";                   
                    if (i < parameters.Length - 1) paramInfo += ", ";
                }
                Console.WriteLine($"{method.ReturnType.Name} {method.Name} ({paramInfo})");
                
             

                
            }

            BankAccount myAccount1 = new BankAccount("John Doe", "123456789", 1000.00m);
            var deposit = typeof(BankAccount).GetMethod("Deposit");
            deposit?.Invoke(myAccount1, new object[] { 500.00m});
        
            Console.ReadLine();

        }
    }
}
