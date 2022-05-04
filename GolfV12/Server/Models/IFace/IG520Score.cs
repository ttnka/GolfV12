﻿using GolfV12.Shared;

namespace GolfV12.Server.Models.IFace
{
    public interface IG520Score
    {
        Task<IEnumerable<G520Score>> Filtro(string? clave);
        Task<G520Score> AddScore(G520Score score);
        Task<G520Score> UpdateScore(G520Score score);
    }
}