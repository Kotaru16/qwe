using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPUStore.Business
{
    public class GPUStoreService
    {
        private readonly List<GPU> _gpus;
        private readonly ValidationService _validationService;

        public GPUStoreService()
        {
            _gpus = new List<GPU>();
            _validationService = new ValidationService();
        }

        public async Task<GPU> AddGPUAsync(GPU gpu)
        {
            if (gpu == null)
                throw new ArgumentNullException(nameof(gpu));

            gpu.Validate();
            _validationService.ValidateGPU(gpu);

            if (_gpus.Any(g => g.Model == gpu.Model && g.Manufacturer == gpu.Manufacturer))
                throw new InvalidOperationException("GPU с такой моделью и производителем уже существует");

            gpu.Id = _gpus.Count > 0 ? _gpus.Max(g => g.Id) + 1 : 1;
            _gpus.Add(gpu);

            return await Task.FromResult(gpu);
        }

        public async Task<GPU> UpdateGPUAsync(GPU gpu)
        {
            if (gpu == null)
                throw new ArgumentNullException(nameof(gpu));

            gpu.Validate();
            _validationService.ValidateGPU(gpu);

            var existingGPU = _gpus.FirstOrDefault(g => g.Id == gpu.Id);
            if (existingGPU == null)
                throw new InvalidOperationException("GPU не найден");

            var duplicateGPU = _gpus.FirstOrDefault(g => 
                g.Id != gpu.Id && 
                g.Model == gpu.Model && 
                g.Manufacturer == gpu.Manufacturer);

            if (duplicateGPU != null)
                throw new InvalidOperationException("GPU с такой моделью и производителем уже существует");

            existingGPU.Model = gpu.Model;
            existingGPU.Manufacturer = gpu.Manufacturer;
            existingGPU.Price = gpu.Price;
            existingGPU.MemorySize = gpu.MemorySize;
            existingGPU.MemoryType = gpu.MemoryType;
            existingGPU.CoreClock = gpu.CoreClock;
            existingGPU.BoostClock = gpu.BoostClock;
            existingGPU.StockQuantity = gpu.StockQuantity;
            existingGPU.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existingGPU);
        }

        public async Task<bool> DeleteGPUAsync(int id)
        {
            var gpu = _gpus.FirstOrDefault(g => g.Id == id);
            if (gpu == null)
                throw new InvalidOperationException("GPU не найден");

            return await Task.FromResult(_gpus.Remove(gpu));
        }

        public async Task<GPU> GetGPUByIdAsync(int id)
        {
            var gpu = _gpus.FirstOrDefault(g => g.Id == id);
            if (gpu == null)
                throw new InvalidOperationException("GPU не найден");

            return await Task.FromResult(gpu);
        }

        public async Task<IEnumerable<GPU>> GetAllGPUsAsync()
        {
            return await Task.FromResult(_gpus.AsEnumerable());
        }

        public async Task<IEnumerable<GPU>> SearchGPUsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllGPUsAsync();

            searchTerm = searchTerm.ToLower();
            return await Task.FromResult(_gpus.Where(g => 
                g.Model.ToLower().Contains(searchTerm) || 
                g.Manufacturer.ToLower().Contains(searchTerm)));
        }

        public async Task<IEnumerable<GPU>> GetGPUsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
                throw new ArgumentException("Минимальная цена не может быть больше максимальной");

            return await Task.FromResult(_gpus.Where(g => g.Price >= minPrice && g.Price <= maxPrice));
        }
    }
} 