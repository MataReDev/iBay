namespace ClassLibrary
{
    public class Cart
    {
        public int Id { get; set; }
        public User user { get; set; }
        public bool isValidated { get; set; }
        public List<Product> listOfProducts { get; set; }
        public DateTime dateValidation { get; set; }
    }
}