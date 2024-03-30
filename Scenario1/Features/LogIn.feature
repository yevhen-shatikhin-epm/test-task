Feature: LogIn

As a carrier provider, I want a username/password login on the tracking website, so that only genuine customers can access their tracking data.

Scenario: User is not logged in 
	Given I’m not logged in with a genuine user
	When I navigate to any page on the tracking site
	Then I am presented with a login screen

Scenario: User can log in with valid credentials 
	Given valid user credentials are already registered
	And I'm on the login screen
	When I enter a valid username and password and submit
	Then I am logged in successfully
