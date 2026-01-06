using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Web.Areas.EQ.Services.Internal;

namespace MatrixNext.Web.Areas.EQ.Services
{
    public class EasyQuoteService
    {
        private readonly EasyQuoteAdapter _adapter;
        private readonly QuoteCalculator _calculator;

        public EasyQuoteService(EasyQuoteAdapter adapter, QuoteCalculator calculator)
        {
            _adapter = adapter;
            _calculator = calculator;
        }

        public EasyQuoteViewModel CargarQuote(long? id)
        {
            if (id == null) return _adapter.NuevaQuote();
            return _adapter.ObtenerQuote(id.Value) ?? _adapter.NuevaQuote();
        }

        public object Guardar(EasyQuoteViewModel vm)
        {
            var calc = _calculator.Calcular(vm);
            vm.Summary = calc;
            var savedId = _adapter.Guardar(vm);
            return new { success = true, id = savedId, summary = calc };
        }

        public EQSummary Calcular(EasyQuoteViewModel vm) => _calculator.Calcular(vm);
    }
}
