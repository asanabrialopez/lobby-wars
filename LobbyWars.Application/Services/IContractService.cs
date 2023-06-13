﻿using LobbyWars.Application.DTOs;
using LobbyWars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Application.Services
{
    public interface IContractService
    {
        Task<EvaluateContractResponseDto> EvaluateContracts(Contract contract);
    }
}
