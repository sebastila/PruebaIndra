using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaIndra.Modelos;

namespace PruebaIndra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebasController : ControllerBase
    {
        [HttpPost(Name = "EstadoCeldas")]
        public ResponseCasas EstadoCeldas(RequestCasas request)
        {
            ResponseCasas response = new ResponseCasas();
			List<int> lstResponse = new List<int>(), temp = new List<int>();

			lstResponse.AddRange(request.LstCasas);
			temp.AddRange(request.LstCasas);
			int diasRestantes = request.Dias;

			while (diasRestantes > 0)
			{
				for (int i = 0; i < 8; i++)
				{
					if (i == 0)
					{
						temp[i] = lstResponse[i + 1];
					}
					else if (i == 7)
					{
						temp[i] = lstResponse[i - 1];
					}
					else
					{
						if (lstResponse[i - 1] == lstResponse[i + 1])
						{
							temp[i] = 0;
						}

						else
						{
							temp[i] = 1;
						}
					}
				}

				if (diasRestantes != 0)
				{
					for (int i = 0; i < 8; i++)
					{
						lstResponse[i] = temp[i];
					}
				}
				diasRestantes--;
			}
			response.Entrada = request.LstCasas;
			response.Salida = lstResponse;
            response.Dias = request.Dias;  

            return response;
        }

		[HttpPost("PaquetesCamion")]
		public List<int> PaquetesCamion(RequestPaquetes request)
		{
			List<int> response = new List<int>();
			List<int> temp = new List<int>();

			int tamanioCarga = request.TamanioCamion - 30;
			temp.AddRange(request.lstPaquetes);
			temp.Sort();
			temp.Reverse();
			Dictionary< int[],int > paquetes = new Dictionary<int[],int >();
			
			if(tamanioCarga > 0)
            {
				for (int i = 0; i < temp.Count - 1; i++)
				{
					for (int j = i + 1; j < temp.Count; j++)

						if (temp[i] + temp[j] <= tamanioCarga)
						{
							int[] parejas = new int[] { temp[i], temp[j] };
							paquetes.Add(parejas, temp[i] + temp[j]);
						}
				}

				int maxValue = paquetes.Values.Max();
				var query = (from x in paquetes
							   where x.Value.Equals(maxValue)
							   select x.Key).FirstOrDefault();

				if (query != null)
                {
					response.AddRange(query);
				}
			}
			return response;
		}
	}
}
