﻿using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Controllers;
using MementoMori.Server.Interfaces;
using MementoMori.Server.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using MementoMori.Server.Models;
using MementoMori.Server;

public class DecksControllerTests
{
    private readonly Mock<IDeckHelper> _mockDeckHelper;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly DecksController _controller;

    public DecksControllerTests()
    {
        _mockDeckHelper = new Mock<IDeckHelper>();
        _mockAuthService = new Mock<IAuthService>();

        _controller = new DecksController(_mockDeckHelper.Object, _mockAuthService.Object);

        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public void View_ReturnsNotFound_WhenDeckNotExists()
    {
        var deckId = Guid.NewGuid();
        _mockDeckHelper
            .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
            .Returns(new List<Deck>());

        var result = _controller.View(deckId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void View_ReturnsBadRequest_WhenGuidEmpty() 
    {
        var deckId = Guid.Empty;
        
        var result = _controller.View(deckId);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void View_ReturnsDeckDTO_WhenDeckExists()
    {
        var deckId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var deck = new Deck
        {
            Id = deckId,
            Title = "Test Deck",
            Creator = new User { Id = creatorId, Username = "TestUser" },
            CardCount = 1,
            Modified = DateOnly.FromDateTime(DateTime.UtcNow),
            Rating = 4.5,
            isPublic = true,
            Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
            Description = "Test Description",
            Cards = new List<Card> { new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" } }
        };
        _mockDeckHelper
            .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
            .Returns(new List<Deck> { deck });
        _mockAuthService
            .Setup(a => a.GetRequesterId(It.IsAny<HttpContext>()))
            .Returns(creatorId);

        var result = _controller.View(deckId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var deckDTO = Assert.IsType<DeckDTO>(okResult.Value);
        Assert.Equal(deckId, deckDTO.Id);
        Assert.Equal("TestUser", deckDTO.CreatorName);
        Assert.Equal(1, deckDTO.CardCount);
        Assert.True(deckDTO.IsOwner);
    }

    [Fact]
    public void EditorView_ReturnsBadRequest_WhenGuidEmpty()
    {
        var deckId = Guid.Empty;

        var result = _controller.EditorView(deckId);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void EditorView_ReturnsNotFound_WhenDeckNotExists()
    {
        var deckId = Guid.NewGuid();
        _mockDeckHelper
            .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
            .Returns(new List<Deck>());

        var result = _controller.EditorView(deckId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void EditorView_ReturnsDeckEditorDTO_WhenDeckExists()
    {
        var deckId = Guid.NewGuid();
        var deck = new Deck
        {
            Id = deckId,
            Title = "Editable Deck",
            isPublic = true,
            Modified = DateOnly.MaxValue,
            CardCount = 2,
            Description = "Editable Description",
            Cards = new List<Card>
        {
            new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" },
            new Card { Id = Guid.NewGuid(), Question = "Q2", Answer = "A2" }
        }
        };

        _mockDeckHelper
            .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
            .Returns(new List<Deck> { deck });

        var result = _controller.EditorView(deckId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var editorDTO = Assert.IsType<DeckEditorDTO>(okResult.Value);

        Assert.Equal(deckId, editorDTO.Id);
        Assert.True(editorDTO.isPublic);
        Assert.Equal(2, editorDTO.CardCount);
        Assert.Equal("Editable Description", editorDTO.Description);
        Assert.Equal("Q1", editorDTO.Cards.First().Question);
    }

    


}