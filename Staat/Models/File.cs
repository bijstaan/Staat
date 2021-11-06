﻿/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.Data;

namespace Staat.Models
{
    public class File : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Namespace { get; set; }
        [Required] public string Hash { get; set; }
        [Required] public string MimeType { get; set; }
        
        // Models that can attach files
        [UseSorting, UseFiltering] public ICollection<Incident> Incidents { get; set; }
        [UseSorting, UseFiltering] public ICollection<IncidentMessage> IncidentMessages { get; set; }
        [UseSorting, UseFiltering] public ICollection<Maintenance> Maintenances { get; set; }
        [UseSorting, UseFiltering] public ICollection<MaintenanceMessage> MaintenanceMessages { get; set; }
        
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}