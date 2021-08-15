/*
 * Staat - Staat
 * Copyright (C) 2021 Matthew Kilgore (tankerkiller125)
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
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using Staat.Models.Users;

namespace Staat.Models
{
    public class Incident : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100), StringLength(100)] public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionHtml { get; set; }
        [Required] public Service Service { get; set; }

        [UseFiltering, UseSorting] public ICollection<IncidentMessage> Messages { get; set; }

        // We do not display the author publicly
        [Required, Authorize] public User Author { get; set; }
        
        [Required] public DateTime StartedAt { get; set; }
        
        [UseFiltering, UseSorting] public ICollection<Models.File> Files { get; set; }
        
        public DateTime? EndedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}