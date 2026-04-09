namespace Service.Implement
{
    public class CategoryInvoiceService : BaseService<CategoryInvoice, ICategoryInvoiceRepository>
    , ICategoryInvoiceService
    {
        private readonly ICategoryInvoiceRepository _CategoryInvoiceRepository;
        public CategoryInvoiceService(ICategoryInvoiceRepository CategoryInvoiceRepository) : base(CategoryInvoiceRepository)
        {
            _CategoryInvoiceRepository = CategoryInvoiceRepository;
        }
    }
}

