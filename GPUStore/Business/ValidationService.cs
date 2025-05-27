using System;
using System.ComponentModel.DataAnnotations;

namespace GPUStore.Business
{
    public class ValidationService
    {
        private readonly string[] _validManufacturers = { "Intel", "AMD" };

        public void ValidateCPU(CPU cpu)
        {
            if (!_validManufacturers.Contains(cpu.Manufacturer, StringComparer.OrdinalIgnoreCase))
                throw new ValidationException($"Неподдерживаемый производитель. Поддерживаемые производители: {string.Join(", ", _validManufacturers)}");

            if (cpu.CoreCount < 2 || cpu.CoreCount > 128)
                throw new ValidationException("Количество ядер должно быть от 2 до 128");

            if (cpu.ThreadCount < cpu.CoreCount)
                throw new ValidationException("Количество потоков не может быть меньше количества ядер");

            if (cpu.BaseClock <= 0 || cpu.BaseClock > 5.0)
                throw new ValidationException("Базовая частота должна быть от 0 до 5 ГГц");

            if (cpu.BoostClock <= 0 || cpu.BoostClock > 6.0)
                throw new ValidationException("Частота разгона должна быть от 0 до 6 ГГц");

            if (cpu.BoostClock < cpu.BaseClock)
                throw new ValidationException("Частота разгона не может быть меньше базовой частоты");

            if (cpu.Price > 1000000)
                throw new ValidationException("Цена не может быть больше 1 000 000");

            if (cpu.StockQuantity > 1000)
                throw new ValidationException("Количество на складе не может быть больше 1000");
        }

        public void ValidateSearchTerm(string searchTerm)
        {
            if (searchTerm != null && searchTerm.Length < 2)
                throw new ValidationException("Поисковый запрос должен содержать минимум 2 символа");
        }

        public void ValidatePriceRange(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
                throw new ValidationException("Минимальная цена не может быть отрицательной");

            if (maxPrice > 1000000)
                throw new ValidationException("Максимальная цена не может быть больше 1 000 000");

            if (minPrice > maxPrice)
                throw new ValidationException("Минимальная цена не может быть больше максимальной");
        }

        public void ValidateCoreCountRange(int minCores, int maxCores)
        {
            if (minCores < 2)
                throw new ValidationException("Минимальное количество ядер не может быть меньше 2");

            if (maxCores > 128)
                throw new ValidationException("Максимальное количество ядер не может быть больше 128");

            if (minCores > maxCores)
                throw new ValidationException("Минимальное количество ядер не может быть больше максимального");
        }
    }
} 