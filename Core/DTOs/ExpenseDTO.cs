
namespace Core.DTOs
{
    public class ExpenseDTO
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        //public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int ExpenseCategoryId { get; set; }
    }

    public class ExpenseResponseDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int ExpenseCategoryId { get; set; }
    }
}