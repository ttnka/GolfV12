﻿using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G522ScoresController : ControllerBase 
    {
        private readonly IG522Scores _scoreIFace;

        public G522ScoresController(IG522Scores scoreIFace)
        {
            this._scoreIFace = scoreIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G522Scores>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _scoreIFace.Filtro(clave);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando de score de un jugador!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<G522Scores>> AddTarjeta(G522Scores score)
        {
            try
            {
                if (score == null) return BadRequest();
                return await _scoreIFace.AddScore(score);
                /*
                 var newTarjeta = await _tarjIFace.AddTarjeta(tarjeta);
                 return CreatedAtAction(nameof(Filtro), new { clave = $"tar1_-_id_-_{tarjeta.Id}" });               
                */
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar agregar un score de un jugador en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G522Scores>> UpdateScore(G522Scores score)
        {
            try
            {
                return score != null ? await _scoreIFace.UpdateScore(score) :
                    NotFound($"El registro del {score.Player} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos en los registros de scores");
            }
        }
    }
}