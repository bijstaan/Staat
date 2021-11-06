Feature: UsersFeature
	User features and information

Scenario: Register a new user
	Given I am a guest user
    When I register a new account
    Then I see an account has been created
    
Scenario: Login a user
	Given I am a guest user
    When I login
    Then I get a valid token