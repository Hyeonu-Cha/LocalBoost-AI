# Product Requirements Document: LocalBoost AI

**Version:** 1.0
**Date:** 28 August 2025
**Author:** Gemini

## 0. Submitting git env
Every epics, userstories are completed, use git command to push into branch.
git add .
git commit -m "[epic/userstory]"
git push

## 1. Vision
To provide small, local businesses in Australia with an affordable and effortless way to improve their local search engine ranking (SEO) and maintain an active social media presence through AI-generated, relevant content.

## 2. User Personas
* **Primary Persona: Maria, the Cafe Owner**
    * **Age:** 45
    * **Role:** Owner of "The Daily Grind Cafe" in Newtown.
    * **Pain Points:** Knows she needs to be on Google and Instagram but has zero time. Doesn't know what to write about. Received a quote for $2000/month from an SEO agency which she can't afford.
    * **Goals:** Get more local customers to find her cafe on Google Maps. Have an active, professional-looking social media feed.

## 3. Features & Requirements (Epics & User Stories)

### Epic 1: Onboarding & Business Profile
*As a user, I need to easily set up my business profile so the AI knows what to write about.*
- **LBA-01:** As Maria, I want a simple signup process for a monthly subscription.
- **LBA-02:** As Maria, I want a guided onboarding wizard to set up my profile.
- **LBA-03:** In the wizard, I want to input my business name, address, and phone number.
- **LBA-04:** In the wizard, I want to describe my business and services in my own words.
- **LBA-05:** In the wizard, I want to select my business category (e.g., Cafe, Mechanic).
- **LBA-06:** In the wizard, I want to specify my target location/suburb.
- **LBA-07:** In the wizard, I want to choose a "tone of voice" (e.g., Professional, Friendly, Humorous).
- **LBA-08:** As a system, I need to save this profile to the user's account.
- **LBA-09:** As Maria, I want to be able to edit my business profile at any time.
- **LBA-10:** As a system, I should provide examples of what to write for the business description.

### Epic 2: AI Content Generation Engine
*As a system, I need to automatically generate content based on the user's profile.*
- **LBA-11:** As a system, I need a scheduled background task that runs weekly for each active user.
- **LBA-12:** The task should first brainstorm a list of relevant blog/social media topics for the user's business type and location.
- **LBA-13:** The task should use the Bing Search API to find local news or events to potentially include.
- **LBA-14:** The task should select the best topic and generate a short blog post (300-400 words).
- **LBA-15:** The task should generate 3-4 corresponding social media posts (e.g., for Instagram, Facebook).
- **LBA-16:** The generated content must incorporate the business name and location naturally for SEO.
- **LBA-17:** The content's tone must match the user's selected "tone of voice."
- **LBA-18:** The system should generate relevant hashtags for the social media posts.
- **LBA-19:** As a system, I need to log the history of generated topics to avoid repetition.
- **LBA-20:** The system must save the generated content in a "pending approval" state.

### Epic 3: Content Review & Approval
*As a user, I need to review, edit, and approve content before it gets used.*
- **LBA-21:** As Maria, I want to receive an email notification when my new weekly content is ready for review.
- **LBA-22:** The email should contain a direct link to the content review page.
- **LBA-23:** On the review page, I want to see the generated blog post and social media snippets.
- **LBA-24:** As Maria, I want to be able to edit the text of the content.
- **LBA-25:** As Maria, I want a single "Approve" button to accept the content.
- **LBA-26:** As Maria, I want a "Regenerate" button if I don't like the content, with an option to provide feedback.
- **LBA-27:** As Maria, I want to be able to easily copy the text for each social media post to my clipboard.
- **LBA-28:** As Maria, my dashboard should show a history of all previously approved content.
- **LBA-29:** As Maria, I want to see the content in a simple, clean interface that is easy to read.
- **LBA-30:** If content is not approved within 7 days, the system should send a reminder email.

### Epic 4: (Future) Automated Publishing
*As a user, I want the system to automatically post the approved content to my accounts.*
- **LBA-31:** As Maria, I want to be able to securely connect my Facebook Business page.
- **LBA-32:** As Maria, I want to be able to securely connect my Instagram account.
- **LBA-33:** As Maria, I want to be able to connect my Google Business Profile.
- **LBA-34:** As Maria, I want to enable an "auto-post" feature for approved content.
- **LBA-35:** When auto-post is enabled, the system should post the content at an optimal time.
- **LBA-36:** As Maria, I need to be able to revoke the system's access to my accounts at any time.
- **LBA-37:** The system must handle API authentication with services like the Meta Graph API.
- **LBA-38:** The system should log the success or failure of each auto-post attempt.
- **LBA-39:** As Maria, I want to receive a notification when content has been successfully posted.
- **LBA-40:** As Maria, I want to be able to post to multiple platforms simultaneously.

## 4. Tech Stack & Necessities
* **Backend:** .NET 9 ASP.NET Core Worker Service (for scheduled generation) and a Web App (for the dashboard).
* **AI Engine:** Azure OpenAI Service (GPT-4 Model).
* **Search Integration:** Bing Search API.
* **Database:** Azure SQL Database.
* **Deployment:** Azure App Service.
* **Frontend:** Blazor Web App.
* **Email:** SendGrid or Azure Communication Services.
* **Payment:** Stripe.
* **Source Control:** Git (GitHub).