﻿// ***********************************************************************
// Assembly         : IronyModManager.Parser.Tests
// Author           : Mario
// Created          : 02-18-2020
//
// Last Modified By : Mario
// Last Modified On : 02-18-2020
// ***********************************************************************
// <copyright file="StellarisFilenameParserTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using IronyModManager.Parser.Stellaris;
using IronyModManager.Tests.Common;
using Xunit;


namespace IronyModManager.Parser.Tests
{
    /// <summary>
    /// Class StellarisFilenameParserTests.
    /// </summary>
    public class StellarisFilenameParserTests
    {
        /// <summary>
        /// Defines the test method CanParse_common_root_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_common_root_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\alerts.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();            
            args.File = "common\\message_types.txt";
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_map_galaxy_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_map_galaxy_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "map\\galaxy\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();            
        }

        /// <summary>
        /// Defines the test method CanParse_on_actions_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_on_actions_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\on_actions\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_weapon_components_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_weapon_components_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\component_templates\\weapon_components.csv",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();            
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_diplo_phrase_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_diplo_phrase_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\diplo_phrases\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_species_names_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_species_names_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\species_names\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_start_screen_messages_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_start_screen_messages_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\start_screen_messages\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_terraform_should_be_true.
        /// </summary>
        [Fact]
        public void CanParse_terraform_should_be_true()
        {
            var args = new CanParseArgs()
            {
                File = "common\\terraform\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method CanParse_should_be_false.
        /// </summary>
        [Fact]
        public void CanParse_should_be_false()
        {
            var args = new CanParseArgs()
            {
                File = "common\\ship_designs\\test.txt",
                GameType = "Stellaris"
            };
            var parser = new FilenameParser();
            parser.CanParse(args).Should().BeFalse();
        }

        /// <summary>
        /// Defines the test method Parse_should_yield_results.
        /// </summary>
        [Fact]
        public void Parse_should_yield_results()
        {
            DISetup.SetupContainer();

            var sb = new StringBuilder();
            sb.AppendLine(@"on_game_start = {");
            sb.AppendLine(@"    events = {");
            sb.AppendLine(@"        dmm_mod_1_flag.1");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");

            var args = new ParserArgs()
            {
                ContentSHA = "sha",
                Dependencies = new List<string> { "1" },
                File = "common\\alerts.txt",
                Lines = sb.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                ModName = "fake"
            };
            var parser = new FilenameParser();
            var result = parser.Parse(args).ToList();
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(1);
            for (int i = 0; i < 1; i++)
            {
                result[i].ContentSHA.Should().Be("sha");
                result[i].Dependencies.First().Should().Be("1");
                result[i].File.Should().Be("common\\alerts.txt");
                switch (i)
                {
                    case 0:
                        result[i].Code.Trim().Should().Be(sb.ToString().Trim());
                        result[i].Id.Should().Be("alerts.txt");
                        result[i].ValueType.Should().Be(ValueType.WholeTextFile);
                        break;

                    default:
                        break;
                }
                result[i].ModName.Should().Be("fake");
                result[i].Type.Should().Be("common");
            }
        }
    }
}
