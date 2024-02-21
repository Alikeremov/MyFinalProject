using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class LayoutService
    {
        private readonly ISettingRepository _repository;

        public LayoutService(ISettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Dictionary<string, string>> GetSettingAsync()
        {
            Dictionary<string, string> settings = await _repository.GetAllnotDeleted().ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
    }
}
