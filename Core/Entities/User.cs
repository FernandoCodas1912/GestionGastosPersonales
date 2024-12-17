  
namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsDeleted { get; set; }


        public List<Expense> Expenses { get; set; } = [];
        public List<ExpenseCategory> ExpenseCategories { get; set; } = [];
    }
}