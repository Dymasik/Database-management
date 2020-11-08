namespace DatabaseManagement.Models
{
    public interface IColumnValue
    {
        Column Column { get; set; }
        string GetValue();
    }
}