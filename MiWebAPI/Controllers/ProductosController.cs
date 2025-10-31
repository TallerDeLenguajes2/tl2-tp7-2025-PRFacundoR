using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
//using ProductoRopositorio;
namespace tl2_tp7_2025_TomaSilva1.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductosController : ControllerBase
{
    private ProductoRepositorio _productoRepositorio;
    public ProductosController()
    {
        _productoRepositorio=new ProductoRepositorio();
    }


    [HttpGet]
    public ActionResult<List<Productos>> GetAll()
    {
        var productos = _productoRepositorio.GetAll();
        return Ok(productos);
    }

    [HttpPost]
    public ActionResult<int> Insertar(Productos prod)
    {
    
        var idNueva = _productoRepositorio.InsertarProducto(prod);
        return Ok(idNueva);
    }
}