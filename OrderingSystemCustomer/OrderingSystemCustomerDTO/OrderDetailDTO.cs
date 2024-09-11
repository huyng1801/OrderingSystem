using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace OrderingSystemCustomerDTO
{
    public class OrderDetailDTO: INotifyPropertyChanged
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá không được âm.")]
        public long UnitePrice { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int _quantity { get; set; }
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isOrdered;

        public bool IsOrdered
        {
            get { return _isOrdered; }
            set
            {
                if (_isOrdered != value)
                {
                    _isOrdered = value;
                    OnPropertyChanged();
                }
            }
        }
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public int ProductID { get; set; }
        public string? ProductName { get; set; } 
        public bool IsServed { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
