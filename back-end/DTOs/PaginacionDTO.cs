﻿namespace back_end.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;

        public int RecordPorPagina
        {
            get
            { 
                return recordsPorPagina; 
            }
            set
            {
                recordsPorPagina = (value >cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }

        }

        public int RecordsPorPagina { get; internal set; }
    }
}
