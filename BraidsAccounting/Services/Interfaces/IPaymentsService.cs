using BraidsAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraidsAccounting.Services.Interfaces
{
    internal interface IPaymentsService
    {
        /// <summary>
        /// Добавить платёж за расходованные материалы.
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        Task AddAsync(Payment payment);
        /// <summary>
        /// Получить общую задолженность сотрудника.
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        Task<decimal> GetDebtAsync(string employeeName);
        /// <summary>
        /// Получить список платежей сотрудника.
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        Task<List<Payment>> GetRangeAsync(string employeeName);
        /// <summary>
        /// Получить сумму платежей сотрудника.
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        Task<decimal> GetTotalAmountAsync(string employeeName);
    }
}