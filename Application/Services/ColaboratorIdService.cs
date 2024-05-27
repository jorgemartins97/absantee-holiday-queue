using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using DataModel.Repository;
using Domain.IRepository;
using Domain.Model;

namespace Application.Services
{
    public class ColaboratorIdService
    {
        private readonly AbsanteeContext _context;

        private readonly IColaboratorsIdRepository _colaboratorsIdRepository;
        
        public ColaboratorIdService(IColaboratorsIdRepository colaboratorsIdRepository) {
            _colaboratorsIdRepository = colaboratorsIdRepository;
        }

        public async Task<ColaboratorIdDTO> Add(ColaboratorIdDTO colaboratorIdDto,List<string> errorMessages)
        {

            ColaboratorId colaboratorId = ColaboratorIdDTO.ToDomain(colaboratorIdDto);
            bool colabExists = await _colaboratorsIdRepository.ColaboratorExists(colaboratorId.colabId);
        
            if(colabExists) {
                errorMessages.Add("Colab already exists");
                return null;
            }

            colaboratorId = await _colaboratorsIdRepository.Add(colaboratorId);
            ColaboratorIdDTO colaboratorIdDTO = ColaboratorIdDTO.ToDTO(colaboratorId);
            return colaboratorIdDTO;
        }
    }
}