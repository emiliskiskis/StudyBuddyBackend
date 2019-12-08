# StudyBuddy

## Backend

* Backend IP address: http://www.buddiesofstudy.tk/ (port 80)
* Content type for request and response will always be JSON (application/json).
* GET requests may not contain a request body and will contain a response body describing the entity or other information as mentioned per endpoint.
* POST, PUT, PATCH requests must contain a request body and will contain a response body fully describing the created/updated entity or other information as mentioned per endpoint.
* DELETE requests may not contain a request body and will contain a response body fully describing the entity that was deleted.
* Please report issues with the back-end. It's likely to be faulty.

### Backend API endpoints
  * `/api/users`
  
      User entity model:
      ```
      {
        "username": Username,
        "password": Password hashed with salt,
        "salt": Salt used with password hashing,
        "firstName": First name,
        "lastName": Last name,
        "email": Email address
      }
      ```
    * `POST /` - add new user  (returns added user)
    * `GET /` - get all users (returns their usernames and names) (requires general authorization)
    * `GET /{username}` - get user (password and salt will not be provided) (requires current user authorization)
    * `PUT /{username}` - update user (returns updated user) (requires current user authorization)
    * `PATCH /{username}` - patch user info partially (returns updated user) (requires current user authorization)
    * `DELETE /{username}` - delete user (returns deleted user) (requires current user authorization)
    
    On password change both password and salt fields have to be updated.
  * `api/auth`
  
    Login request model:
    ```
    {
      "username": Username of user,
      "password": Password of user hashed with salt
    }
    ```
    
    Salt response model:
    ```
    {
      "salt": Salt used with password hashing
    }
    ```
    * `POST /login` - request to login to server (200 OK if logged in)
    * `GET /salt/{username}` - get user salt
  * `api/chat`
  
    Group request model:
    ```
    {
      "username": Username of user who's connecting,
      "connectTo": Username of user to connect to
    }
    ```
    
    Chat response model:
    ```
    [
      {
        "id": ID of group, a GUID string (readonly),
        "lastMessage": { Message JSON },
        "name": Name of chat (optional),
        "users: [ { Public user JSON }, ... ]
      },
      ...
    ]
    ```
    
    Messages response model:
    ```
    [
      {
        "id": Message GUID,
        "user": { Public user JSON },
        "text": Text of message,
        "sentAt": Datetime of message sent
      },
      ...
    ]
    ```
    * `POST /` - create chat (returns created chat); if chat between exactly two users already exists, returns existing chat (requires current user authentication)
    * `GET /{username}` - get chats of user (requires current user authentication)
    * `GET /{id}/messages` - get messages of chat by id (requires user to be in chat)
