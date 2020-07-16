#region LIBS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;

#endregion

//Este es un web service RestAPI sin custom "throws".

namespace ContactosMavidavidBlancosController.API.Controllers
{
    //Aqui se asigna la ruta
    [Route("api/[controller]")]
    //Nos permite no tener que esspecificar el [fromBody]
    [ApiController]
    //Esta clase hereda de controllerBase la clase que nos permite trabajar con las 
    //tipicas funciones de un controller sin depender forzosamente del motor de vistas razor 

    public class ContactosMavidavidBlancosController : ControllerBase
    {
        #region DB CONTEXT/AUTO_MAPPER
        //Se declara un atributo de tipo obj readonly de la clase MaviContext 
        //para poder hacer usa de la bd dentro de la clase del controlador ya que la clase MaviContext hereda a su vez
        //de DbContext que aplica la interfaz diposable entre otras que nos permiten 
        //trabajar con la bd por medio del ORM EntityFrmaworkCore, a su vez se 
        //le pasa por parametros al un obj tipo  MaviContext al contructor de la clase del controller 

        private readonly MaviContext _context;
        private readonly IMapper _mapper;


        public ContactosMavidavidBlancosController(MaviContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region GET ALL
        //Este metodo es el endpoint "get all", es asincrono y devuelve todos los registros
        //de la tabla ContactosMavidavidBlancos en caso de que tenga registros devuelve un 200(junto con los datos en formato JSON)
        //en caso contrario devuelve un 404 pasando antes por el "DTO matcher".
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactosMavidavidBlancosDTO>>> GetContacts()
        {

            try
            {
                var contactObj = await _context.ContactosMavidavidBlancos.
                    Select(x => new ContactosMavidavidBlancosDTO
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        TelefonoFijo = x.TelefonoFijo,
                        TipoContacto = x.TipoContacto

                    }).ToListAsync();

                if (contactObj.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(contactObj);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }
        }

        #endregion

        #region GET BY LIKE
        //Este metodo es un endpoint creado para atender busquedas, es un tercer endpoint get al cuál se accede por medio del nombre
        //del metodo junto con el "nombre" a recibir , es asincrono y devuelve todos los registros
        //de la tabla ContactosMavidavidBlancos que hagan match con el parametro "name" en caso de que tenga registros devuelve un 200(junto con los datos en formato DTO JSON)
        //en caso contrario devuelve un 404 .
        [Route("[action]/{name}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactosMavidavidBlancosDTO>>> GetContactsByName(string name)
        {

            try
            {
                var contactObj = await _context.ContactosMavidavidBlancos.Where(c => c.Nombre.Contains(name)).
                    Select(x => new ContactosMavidavidBlancosDTO
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        TelefonoFijo = x.TelefonoFijo,
                        TipoContacto = x.TipoContacto

                    }).ToListAsync();

                if (contactObj.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(contactObj);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }
        }

        #endregion

        #region GET ONE
        //Este metodo es el endpoint "get one", es asincrono y devuelve un registro
        //basado en el id de la tabla ContactosMavidavidBlancos en caso de que tenga el registro devuelve un 200 (junto con los datos en formato JSON)
        //en caso contrario devuelve un 404 o un 500 en caso de error, pasando antes
        // por el "DTO matcher" en caso de que el id exista en la bd.
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactosMavidavidBlancosDTO>> GetContact(int id)
        {
            try
            {

                var contactObj = await _context.ContactosMavidavidBlancos.FindAsync(id);
                if (contactObj == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ContactosMavidavidBlancosDTO>(contactObj));
            }


            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region UPDATE
        //Este metodo es el endpoint "update", es asincrono y devuelve un 204 en caso
        //caso de que el registro a editar haya sido encontrado por medio del id y editado
        //o un 404 en caso contrario o un 500 en caso de algun error de conexion u otro

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactUpdateDto contactUpdateDtoObj)
        {

            try
            {
                var contactObj = await _context.ContactosMavidavidBlancos.FindAsync(id);
                if (contactObj == null)
                {
                    return NotFound();
                }

                _mapper.Map(contactUpdateDtoObj, contactObj);

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException) when (ContactosMavidavidBlancosExists(id))
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region CREATE
        //Este metodo es el "endpoint" crear o insertar, donde antes que nada convierte de 
        //dto obj al obj de la entidad para proceder a guardarlo en la bd de manera asincrona
        //retornando un 204 en caso de que la transaccion haya sido exitosa o un 500 en caso contrario
        [HttpPost]
        public async Task<ActionResult<ContactosMavidavidBlancosDTO>> CreateContact(ContactosMavidavidBlancosDTO contactObjDtoObj)
        {

            try
            {

                var contactObj = _mapper.Map<ContactosMavidavidBlancos>(contactObjDtoObj);

                _context.ContactosMavidavidBlancos.Add(contactObj);
                //El SaveChangesAsync es como un "commit" pero asincrono
                await _context.SaveChangesAsync();

                var contactDtoObj = _mapper.Map<ContactosMavidavidBlancosDTO>(contactObj);

                //El NoContent regresa el 204
                return CreatedAtAction(
                    nameof(GetContact),
                    new { id = contactObj.Id },
                    contactDtoObj
                );

            }
            catch (Exception)
            {

                return StatusCode(500);
            }

        }

        #endregion

        #region DELETE
        //Este endpoint es el delete, primero busca el registro a eliminar 
        //luego en caso de que no exista devuelve un 404
        //En caso contrario devuelve un 204, una respuesta sin contenido

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var contactObj = await _context.ContactosMavidavidBlancos.FindAsync(id);
                if (contactObj == null)
                {
                    return NotFound();
                }
                //Este es un delete en EF Core
                _context.ContactosMavidavidBlancos.Remove(contactObj);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region DTO MATCHER
        //Este metodo valida la existencia de un registro por medio de su id, devolviedo
        //un booleano(true/false) en caso de que lo encuentre
        private bool ContactosMavidavidBlancosExists(int id) =>
                _context.ContactosMavidavidBlancos.Any(e => e.Id == id);

        #endregion

    }

}

//agregr date y select a los inputs del cliente
//agregar el buscar (nuevo endpoint)