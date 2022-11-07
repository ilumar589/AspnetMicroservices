using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<CuponModel> CreateDisocount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cupon = _mapper.Map<Cupon>(request.Cupon);

            await _repository.CreateDiscount(cupon);

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", cupon.ProductName);

            return request.Cupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDisocount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse { Success = deleted };
        }

        public override async Task<CuponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var cupon = await _repository.GetDiscount(request.ProductName);
            if (cupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not present"));
            }

            _logger.LogInformation("Discount is retrieved for ProductName: {ProductName}, Amount: {Amount}", cupon.ProductName, cupon.Amount);

            var cuponModel = _mapper.Map<CuponModel>(cupon);
            return cuponModel;
        }

        public override async Task<CuponModel> UpdateDisocount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var cupon = _mapper.Map<Cupon>(request.Cupon);
            await _repository.UpdateDiscount(cupon);
            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", cupon.ProductName);

            return request.Cupon;
        }
    }
}
