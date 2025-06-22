{
  "openapi": "3.0.4",
  "info": {
    "title": "Nasteafy",
    "version": "1.0"
  },
  "paths": {
    "/api/users/profile": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "profileGET",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Users.Queries.GetById.GetUserResponse"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      },
      "put": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "profilePUT",
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Users.Commands.Update.UpdateProfileCommand"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/tracks": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "tracksPOST",
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Tracks.Commands.Create.CreateTrackCommand"
              }
            },
            "application/x-www-form-urlencoded": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Tracks.Commands.Create.CreateTrackCommand"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/tracks/{albumId}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "tracksGET",
        "parameters": [
          {
            "name": "albumId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/tracks/{artistId}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "tracksGET2",
        "parameters": [
          {
            "name": "artistId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/tracks/{Id}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "tracksGET3",
        "parameters": [
          {
            "name": "Id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/tracks/{playlistId}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "tracksGET4",
        "parameters": [
          {
            "name": "playlistId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/subscriptions/cancel": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "cancel",
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/subscriptions": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "subscriptionsGET",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Subscriptions.Queries.GetAll.GetAllSubscriptionsResponse"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      },
      "put": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "subscriptionsPUT",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Subscriptions.Commands.Update.UpdateSubscriptionCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/subscriptions/{Id}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "subscriptionsGET2",
        "parameters": [
          {
            "name": "Id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Subscriptions.Queries.GetAll.GetSubscriptionDto"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/users/subscribe": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "subscribe",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Subscriptions.Commands.SubscribeUserCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/playlists": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "playlistsPOST",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Playlists.Commands.Create.CreatePlaylistCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "string",
                  "format": "uuid"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      },
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "playlistsGET",
        "parameters": [
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Playlists.Queries.GetByUserId.UserPlaylistDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/playlists/{playlistId}": {
      "delete": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "playlistsDELETE",
        "parameters": [
          {
            "name": "playlistId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/playlists/{Id}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "playlistsGET2",
        "parameters": [
          {
            "name": "Id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Playlists.Queries.GetByUserId.UserPlaylistDto"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/playlists/{id}": {
      "put": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "playlistsPUT",
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Playlists.Commands.Update.UpdatePlaylistCommand"
              }
            },
            "application/x-www-form-urlencoded": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Playlists.Commands.Update.UpdatePlaylistCommand"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/auth/login": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "login",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Auth.Commands.Login.LoginCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "string"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/auth/logout": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "logout",
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/auth/refresh": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "refresh",
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "string"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/auth/register": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "register",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Auth.Commands.Register.RegisterCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/artists": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "artistsPOST",
        "requestBody": {
          "content": {
            "multipart/form-data": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Artists.Commands.Create.CreateArtistCommand"
              }
            },
            "application/x-www-form-urlencoded": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Artists.Commands.Create.CreateArtistCommand"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "string",
                  "format": "uuid"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      },
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "artistsGET",
        "parameters": [
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Artists.Queries.GetAll.ArtistDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/artists/{artistId}": {
      "delete": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "artistsDELETE",
        "parameters": [
          {
            "name": "artistId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/artists/{Id}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "artistsGET2",
        "parameters": [
          {
            "name": "Id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Artists.Queries.GetAll.ArtistDto"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/albums": {
      "post": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "albumsPOST",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Nasteafy.Application.Albums.Commands.Create.CreateAlbumCommand"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "type": "string",
                  "format": "uuid"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/albums/{artistId}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "albumsGET",
        "parameters": [
          {
            "name": "artistId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          },
          {
            "name": "PageNumber",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Albums.Queries.GetById.AlbumDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    },
    "/api/albums/{Id}": {
      "get": {
        "tags": [
          "Nasteafy"
        ],
        "operationId": "albumsGET2",
        "parameters": [
          {
            "name": "Id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string",
              "format": "uuid"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Albums.Queries.GetById.AlbumDto"
                }
              }
            }
          },
          "400": {
            "description": "Bad Request",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Nasteafy.Application.Common.Models.ApiError"
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Nasteafy.Application.Albums.Commands.Create.CreateAlbumCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "title": {
            "type": "string",
            "nullable": true
          },
          "coverFile": {
            "type": "string",
            "format": "binary",
            "nullable": true
          },
          "releaseDate": {
            "type": "string",
            "format": "date-time"
          },
          "artists": {
            "type": "array",
            "nullable": true,
            "items": {
              "type": "string",
              "format": "uuid"
            }
          }
        }
      },
      "Nasteafy.Application.Albums.Queries.GetById.AlbumDto": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "coverUrl": {
            "type": "string",
            "nullable": true
          },
          "artist": {
            "type": "string",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Artists.Commands.Create.CreateArtistCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "name": {
            "type": "string",
            "nullable": true
          },
          "artistPhoto": {
            "type": "string",
            "format": "binary",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Artists.Queries.GetAll.ArtistDto": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "avatarUrl": {
            "type": "string",
            "nullable": true
          },
          "name": {
            "type": "string",
            "nullable": true
          },
          "isVerified": {
            "type": "boolean"
          }
        }
      },
      "Nasteafy.Application.Auth.Commands.Login.LoginCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "email": {
            "type": "string",
            "nullable": true
          },
          "password": {
            "type": "string",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Auth.Commands.Register.RegisterCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "email": {
            "type": "string",
            "nullable": true
          },
          "password": {
            "type": "string",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Common.Models.ApiError": {
        "type": "object",
        "additionalProperties": false,
        "required": [
          "errorMessage"
        ],
        "properties": {
          "statusCode": {
            "type": "integer",
            "format": "int32"
          },
          "errorMessage": {
            "type": "string",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Albums.Queries.GetById.AlbumDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "items": {
            "type": "array",
            "nullable": true,
            "items": {
              "$ref": "#/components/schemas/Nasteafy.Application.Albums.Queries.GetById.AlbumDto"
            }
          },
          "totalItems": {
            "type": "integer",
            "format": "int32"
          },
          "pageNumber": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "readOnly": true,
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Artists.Queries.GetAll.ArtistDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "items": {
            "type": "array",
            "nullable": true,
            "items": {
              "$ref": "#/components/schemas/Nasteafy.Application.Artists.Queries.GetAll.ArtistDto"
            }
          },
          "totalItems": {
            "type": "integer",
            "format": "int32"
          },
          "pageNumber": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "readOnly": true,
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Playlists.Queries.GetByUserId.UserPlaylistDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "items": {
            "type": "array",
            "nullable": true,
            "items": {
              "$ref": "#/components/schemas/Nasteafy.Application.Playlists.Queries.GetByUserId.UserPlaylistDto"
            }
          },
          "totalItems": {
            "type": "integer",
            "format": "int32"
          },
          "pageNumber": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "readOnly": true,
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Common.Models.PagedResult`1[[Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto, Nasteafy.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "items": {
            "type": "array",
            "nullable": true,
            "items": {
              "$ref": "#/components/schemas/Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto"
            }
          },
          "totalItems": {
            "type": "integer",
            "format": "int32"
          },
          "pageNumber": {
            "type": "integer",
            "format": "int32"
          },
          "pageSize": {
            "type": "integer",
            "format": "int32"
          },
          "totalPages": {
            "type": "integer",
            "readOnly": true,
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Playlists.Commands.Create.CreatePlaylistCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "title": {
            "type": "string",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Playlists.Commands.Update.UpdatePlaylistCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "playlistId": {
            "type": "string",
            "format": "uuid"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "coverFile": {
            "type": "string",
            "format": "binary",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Playlists.Queries.GetByUserId.UserPlaylistDto": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "coverUrl": {
            "type": "string",
            "nullable": true
          },
          "tracksCount": {
            "type": "integer",
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Subscriptions.Commands.SubscribeUserCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "subscriptionId": {
            "type": "string",
            "format": "uuid"
          }
        }
      },
      "Nasteafy.Application.Subscriptions.Commands.Update.UpdateSubscriptionCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "price": {
            "type": "number",
            "format": "double"
          },
          "durationInDays": {
            "type": "integer",
            "format": "int32"
          }
        }
      },
      "Nasteafy.Application.Subscriptions.Queries.GetAll.GetAllSubscriptionsResponse": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "subscriptions": {
            "type": "array",
            "nullable": true,
            "items": {
              "$ref": "#/components/schemas/Nasteafy.Application.Subscriptions.Queries.GetAll.GetSubscriptionDto"
            }
          }
        }
      },
      "Nasteafy.Application.Subscriptions.Queries.GetAll.GetSubscriptionDto": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "name": {
            "type": "string",
            "nullable": true
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "price": {
            "type": "number",
            "format": "double"
          }
        }
      },
      "Nasteafy.Application.Tracks.Commands.Create.CreateTrackCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "file": {
            "type": "string",
            "format": "binary",
            "nullable": true
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "duration": {
            "type": "string",
            "format": "date-span"
          },
          "albumId": {
            "type": "string",
            "format": "uuid",
            "nullable": true
          },
          "artistIds": {
            "type": "array",
            "nullable": true,
            "items": {
              "type": "string",
              "format": "uuid"
            }
          }
        }
      },
      "Nasteafy.Application.Tracks.Queries.GetById.GetTrackDto": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "id": {
            "type": "string",
            "format": "uuid"
          },
          "title": {
            "type": "string",
            "nullable": true
          },
          "artistName": {
            "type": "string",
            "nullable": true
          },
          "pathUrl": {
            "type": "string",
            "nullable": true
          },
          "duration": {
            "type": "string",
            "format": "date-span"
          }
        }
      },
      "Nasteafy.Application.Users.Commands.Update.UpdateProfileCommand": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "email": {
            "type": "string",
            "nullable": true
          },
          "userName": {
            "type": "string",
            "nullable": true
          },
          "avatarFile": {
            "type": "string",
            "format": "binary",
            "nullable": true
          }
        }
      },
      "Nasteafy.Application.Users.Queries.GetById.GetUserResponse": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "email": {
            "type": "string",
            "nullable": true
          },
          "userName": {
            "type": "string",
            "nullable": true
          },
          "avatarUrl": {
            "type": "string",
            "nullable": true
          },
          "subscriptionType": {
            "$ref": "#/components/schemas/Nasteafy.Domain.Entities.Subscriptions.SubscriptionType"
          }
        }
      },
      "Nasteafy.Domain.Entities.Subscriptions.SubscriptionType": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "name": {
            "type": "string",
            "nullable": true
          }
        }
      }
    },
    "securitySchemes": {
      "Bearer": {
        "type": "http",
        "description": "Enter your JWT token in this field",
        "scheme": "Bearer",
        "bearerFormat": "JWT"
      }
    }
  },
  "security": [
    {
      "Bearer": []
    }
  ]
}