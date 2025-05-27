using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPUStore.Business
{
    public class CPUStoreService
    {
        private readonly List<CPU> _cpus;
        private readonly ValidationService _validationService;

        public CPUStoreService()
        {
            _cpus = new List<CPU>();
            _validationService = new ValidationService();
        }

        public async Task<CPU> AddCPUAsync(CPU cpu)
        {
            if (cpu == null)
                throw new ArgumentNullException(nameof(cpu));

            cpu.Validate();
            _validationService.ValidateCPU(cpu);

            if (_cpus.Any(c => c.Model == cpu.Model && c.Manufacturer == cpu.Manufacturer))
                throw new InvalidOperationException("Процессор с такой моделью и производителем уже существует");

            cpu.Id = _cpus.Count > 0 ? _cpus.Max(c => c.Id) + 1 : 1;
            _cpus.Add(cpu);

            return await Task.FromResult(cpu);
        }

        public async Task<CPU> UpdateCPUAsync(CPU cpu)
        {
            if (cpu == null)
                throw new ArgumentNullException(nameof(cpu));

            cpu.Validate();
            _validationService.ValidateCPU(cpu);

            var existingCPU = _cpus.FirstOrDefault(c => c.Id == cpu.Id);
            if (existingCPU == null)
                throw new InvalidOperationException("Процессор не найден");

            var duplicateCPU = _cpus.FirstOrDefault(c => 
                c.Id != cpu.Id && 
                c.Model == cpu.Model && 
                c.Manufacturer == cpu.Manufacturer);

            if (duplicateCPU != null)
                throw new InvalidOperationException("Процессор с такой моделью и производителем уже существует");

            existingCPU.Model = cpu.Model;
            existingCPU.Manufacturer = cpu.Manufacturer;
            existingCPU.Price = cpu.Price;
            existingCPU.CoreCount = cpu.CoreCount;
            existingCPU.ThreadCount = cpu.ThreadCount;
            existingCPU.BaseClock = cpu.BaseClock;
            existingCPU.BoostClock = cpu.BoostClock;
            existingCPU.StockQuantity = cpu.StockQuantity;
            existingCPU.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existingCPU);
        }

        public async Task<bool> DeleteCPUAsync(int id)
        {
            var cpu = _cpus.FirstOrDefault(c => c.Id == id);
            if (cpu == null)
                throw new InvalidOperationException("Процессор не найден");

            return await Task.FromResult(_cpus.Remove(cpu));
        }

        public async Task<CPU> GetCPUByIdAsync(int id)
        {
            var cpu = _cpus.FirstOrDefault(c => c.Id == id);
            if (cpu == null)
                throw new InvalidOperationException("Процессор не найден");

            return await Task.FromResult(cpu);
        }

        public async Task<IEnumerable<CPU>> GetAllCPUsAsync()
        {
            return await Task.FromResult(_cpus.AsEnumerable());
        }

        public async Task<IEnumerable<CPU>> SearchCPUsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCPUsAsync();

            searchTerm = searchTerm.ToLower();
            return await Task.FromResult(_cpus.Where(c => 
                c.Model.ToLower().Contains(searchTerm) || 
                c.Manufacturer.ToLower().Contains(searchTerm)));
        }

        public async Task<IEnumerable<CPU>> GetCPUsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
                throw new ArgumentException("Минимальная цена не может быть больше максимальной");

            return await Task.FromResult(_cpus.Where(c => c.Price >= minPrice && c.Price <= maxPrice));
        }

        public async Task<IEnumerable<CPU>> GetCPUsByCoreCountAsync(int minCores, int maxCores)
        {
            if (minCores > maxCores)
                throw new ArgumentException("Минимальное количество ядер не может быть больше максимального");

            return await Task.FromResult(_cpus.Where(c => c.CoreCount >= minCores && c.CoreCount <= maxCores));
        }
    }
} 