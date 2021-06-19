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
using HotChocolate.Data;

namespace Staat.Models
{
    public class ServiceGroup : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100), StringLength(100)] public string Name { get; set; }
        [MaxLength(255), StringLength(255)] public string Description { get; set; }
        public bool DefaultOpen { get; set; }

        [UseFiltering, UseSorting] public ICollection<Service> Services { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}