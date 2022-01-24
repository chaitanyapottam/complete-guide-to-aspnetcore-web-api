using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        PublishersService _publishersService;

        public PublishersController(PublishersService publishersService)
        {
            _publishersService = publishersService;
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody]PublisherVM publisher)
        {            
            try
            {
                var _newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), _newPublisher);
            }
            catch(PublisherNameException ex) 
            {
                return BadRequest($"{ex.Message}, Publisher Name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-by-id/{Id}")]
        public IActionResult GetPublisherById(int Id)
        {
            var _response = _publishersService.GetPublisherById(Id);
            if(_response != null)
            {
                return Ok(_response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-publisher-books-with-authors/{Id}")]
        public IActionResult GetPublisherData(int Id)
        {
            var response = _publishersService.GetPublisherData(Id);
            return Ok(response);
        }

        [HttpDelete("delete-publisher-by-id/{Id}")]
        public IActionResult DeletePublisherById(int Id)
        {
            try
            {
                _publishersService.DeletePublisherById(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
