namespace NSE.Pedido.API.Controllers;

public class VoucherController : MainController
{
    private readonly IVoucherQueries _voucherQueries;

    public VoucherController(IVoucherQueries voucherQueries) => _voucherQueries = voucherQueries;

    [HttpGet("voucher/{code}")]
    [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return NotFound();

        var voucher = await _voucherQueries.GetVoucherByCode(code);

        return voucher is null ? NotFound() : CustomResponse(voucher);
    }
}
