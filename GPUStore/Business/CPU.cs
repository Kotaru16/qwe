using System;
using System.ComponentModel.DataAnnotations;

namespace GPUStore.Business
{
    public class CPU : IEntity
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int CoreCount { get; set; }
        public int ThreadCount { get; set; }
        public double BaseClock { get; set; }
        public double BoostClock { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public CPU()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Model))
                throw new ValidationException("Модель процессора не может быть пустой");

            if (string.IsNullOrWhiteSpace(Manufacturer))
                throw new ValidationException("Производитель не может быть пустым");

            if (Price <= 0)
                throw new ValidationException("Цена должна быть больше 0");

            if (CoreCount < 2 || CoreCount > 128)
                throw new ValidationException("Количество ядер должно быть от 2 до 128");

            if (ThreadCount < CoreCount)
                throw new ValidationException("Количество потоков не может быть меньше количества ядер");

            if (BaseClock <= 0 || BaseClock > 5.0)
                throw new ValidationException("Базовая частота должна быть от 0 до 5 ГГц");

            if (BoostClock <= 0 || BoostClock > 6.0)
                throw new ValidationException("Частота разгона должна быть от 0 до 6 ГГц");

            if (BoostClock < BaseClock)
                throw new ValidationException("Частота разгона не может быть меньше базовой частоты");

            if (StockQuantity < 0)
                throw new ValidationException("Количество на складе не может быть отрицательным");

            return true;
        }

        public void UpdateStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                throw new InvalidOperationException("Недостаточно товара на складе");
            
            StockQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public decimal CalculateDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Процент скидки должен быть от 0 до 100");

            return Price * (1 - discountPercentage / 100);
        }
    }
} 