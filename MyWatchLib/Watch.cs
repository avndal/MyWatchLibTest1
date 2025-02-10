namespace MyWatchLib
{
    public class Watch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
       

        public void ValidateName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentNullException("Name cannot be empty");
            }
            if (Name.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Name length must be over 1");
            }
        }
        public void ValidatePrice()
        {
            if (Price < 148)
            {
                throw new ArgumentOutOfRangeException("Price must be bigger than 148 kr");
            }
        }

        public void ValidateModel()
        {

            if (string.IsNullOrEmpty(Model))
            {
                throw new ArgumentNullException("Model is needed");
            }
            if (Model.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Model length has to be bigger than 1");
            }
        }

        public void Validate()
        {
            ValidateName();
            ValidateModel();
            ValidatePrice();
        }
    }
}
