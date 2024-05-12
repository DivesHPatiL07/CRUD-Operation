namespace CRUD_Operation.Models
{
    public class ProductListModel
    {
        public List<ProductDataModel> rows { get; set; }
        public int total { get; set; }
        public ProductListModel()
        {
            rows = new List<ProductDataModel>();
        }
    }
    public class ProductDataModel
    {
        public int ID { get; set; }
        public string? PRODUCT { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? CREATEDDATE { get; set; }
    }
}
