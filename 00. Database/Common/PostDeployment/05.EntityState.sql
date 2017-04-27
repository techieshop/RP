INSERT INTO [plt].[EntityState] ([Id], [NameCode], [DescriptionCode], [Order], [IsStart], [IsFinish], [IsActive], [EntityTypeId]) VALUES 
-- User
(1, 306, NULL, 1, 1, 0, 0, 1), /* Created */
(2, 307, NULL, 2, 0, 0, 1, 1), /* Active */
(3, 308, NULL, 3, 0, 0, 0, 1), /* Blocked */
(4, 309, NULL, 4, 0, 1, 0, 1), /* Deleted */

--Organization
(5, 310, NULL, 1, 1, 0, 0, 2), /* Created */
(6, 311, NULL, 2, 0, 0, 1, 2), /* Active */
(7, 312, NULL, 3, 0, 0, 1, 2), /* Inactive */
(8, 313, NULL, 4, 0, 0, 0, 2), /* Blocked */
(9, 314, NULL, 5, 0, 0, 0, 2), /* Closed */
(10,315, NULL, 6, 0, 1, 0, 2), /* Deleted */

--Member
(11, 316, NULL, 1, 1, 0, 0, 3), /* Created */
(12, 317, NULL, 2, 0, 0, 1, 3), /* Active */
(13, 318, NULL, 3, 0, 0, 1, 3), /* Inactive */
(14, 319, NULL, 4, 0, 0, 0, 3), /* Blocked */
(15, 320, NULL, 5, 0, 1, 0, 3), /* Deleted */

--Pigeon
(16, 321, NULL, 1, 1, 0, 0, 4), /* Created */
(17, 322, NULL, 2, 0, 0, 1, 4), /* Active */
(18, 323, NULL, 3, 0, 0, 1, 4), /* Inactive */
(19, 324, NULL, 4, 0, 0, 0, 4), /* Blocked */
(20, 325, NULL, 5, 0, 1, 0, 4), /* Deleted */

--Season
(21, 326, NULL, 1, 1, 0, 0, 5), /* Created */
(22, 327, NULL, 2, 0, 0, 1, 5), /* Open */
(23, 328, NULL, 3, 0, 0, 1, 5), /* Closed */
(24, 329, NULL, 4, 0, 1, 0, 5), /* Deleted */

--Race
(25, 330, NULL, 1, 1, 0, 0, 6), /* Created */
(26, 331, NULL, 2, 0, 0, 1, 6), /* Open */
(27, 332, NULL, 3, 0, 0, 1, 6), /* Closed */
(28, 333, NULL, 4, 0, 1, 0, 6), /* Deleted */

--Result
(29, 334, NULL, 1, 1, 0, 0, 7), /* Created */
(30, 335, NULL, 2, 0, 0, 1, 7), /* Published */
(31, 336, NULL, 3, 0, 1, 0, 7), /* Deleted */

-- Role
(32, 337, NULL, 1, 1, 0, 0, 8), /* Created */
(33, 338, NULL, 2, 0, 0, 1, 8), /* Active */
(34, 339, NULL, 3, 0, 0, 0, 8), /* Inactive */
(35, 340, NULL, 4, 0, 1, 0, 8), /* Deleted */

-- Template
(36, 341, NULL, 1, 1, 0, 0, 9), /* Created */
(37, 342, NULL, 2, 0, 0, 1, 9), /* Active */
(38, 343, NULL, 3, 0, 0, 0, 9), /* Inactive */
(39, 344, NULL, 4, 0, 1, 0, 9), /* Deleted */

-- Point
(40, 358, NULL, 1, 1, 0, 0, 10), /* Created */
(41, 359, NULL, 2, 0, 0, 1, 10), /* Active */
(42, 360, NULL, 3, 0, 0, 1, 10), /* Inactive */
(43, 361, NULL, 4, 0, 1, 0, 10) /* Deleted */