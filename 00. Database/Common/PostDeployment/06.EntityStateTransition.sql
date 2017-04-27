INSERT INTO [plt].[EntityStateTransition] ([Id], [ActionBeforeCode], [ActionAfterCode], [Order], [EntityStateFromId], [EntityStateToId]) VALUES 
-- User
(1, 701, 601, 1, NULL,	1), /* NULL -> Created */
(2, 702, 602, 2, 1,	2), /* Created -> Active */
(3, 703, 603, 3, 1,	4), /* Created -> Deleted */
(4, 704, 604, 4, 2,	3), /* Active -> Blocked */
(5, 705, 605, 5, 2,	4), /* Active -> Deleted */
(6, 706, 606, 6, 3,	2), /* Blocked -> Active */
(7, 707, 607, 7, 3,	4), /* Blocked -> Deleted */

-- Organization
(8,	 708, 608, 1,	NULL,	5), /* NULL -> Created */
(9,	 709, 609, 2,	5,		6), /* Created -> Active */
(10, 710, 610, 3,	5,		10), /* Created -> Deleted */
(11, 711, 611, 4,	6,		7), /* Active -> Inactive */
(12, 712, 612, 5,	6,		8), /* Active -> Blocked */
(13, 713, 613, 6,	6,		9), /* Active -> Closed */
(14, 714, 614, 7,	6,		10), /* Active -> Deleted */
(15, 715, 615, 8,	7,		6), /* Inactive -> Active */
(16, 716, 616, 9,	7,		12), /* Inactive -> Blocked */
(17, 717, 617, 10,	7,		13), /* Inactive -> Closed */
(18, 718, 618, 11,	7,		10), /* Inactive -> Deleted */
(19, 719, 619, 12,	8,		6), /* Blocked -> Active */
(20, 720, 620, 13,	8,		7), /* Blocked -> Inactive */
(21, 721, 621, 14,	8,		9), /* Blocked -> Closed */
(22, 722, 622, 15,	8,		10), /* Blocked -> Deleted */
(23, 723, 623, 16,	9,		6), /* Closed -> Active */
(24, 724, 624, 17,	9,		7), /* Closed -> Inactive */
(25, 725, 625, 18,	9,		8), /* Closed -> Blocked */
(26, 726, 626, 19,	9,		10), /* Closed -> Deleted */

-- Member
(27, 727, 627, 1,	NULL,	11), /* NULL -> Created */
(28, 728, 628, 2,	11,		12), /* Created -> Active */
(29, 729, 629, 3,	11,		15), /* Created -> Deleted */
(30, 730, 630, 4,	12,		13), /* Active -> Inactive */
(31, 731, 631, 5,	12,		14), /* Active -> Blocked */
(32, 732, 632, 6,	12,		15), /* Active -> Deleted */
(33, 733, 633, 7,	13,		12), /* Inactive -> Active */
(34, 734, 634, 8,	13,		14), /* Inactive -> Blocked */
(35, 735, 635, 9,	13,		15), /* Inactive -> Deleted */
(36, 736, 636, 10,	14,		12), /* Blocked -> Active */
(37, 737, 637, 11,	14,		13), /* Blocked -> Inactive */
(38, 738, 638, 12,	14,		15), /* Blocked -> Deleted */

-- Pigeon
(39, 739, 639, 1,	NULL,	16), /* NULL -> Created */
(40, 740, 640, 2,	16,		17), /* Created -> Active */
(41, 741, 641, 3,	16,		20), /* Created -> Deleted */
(42, 742, 642, 4,	17,		18), /* Active -> Inactive */
(43, 743, 643, 5,	17,		19), /* Active -> Blocked */
(44, 744, 644, 6,	17,		20), /* Active -> Deleted */
(45, 745, 645, 7,	18,		17), /* Inactive -> Active */
(46, 746, 646, 8,	18,		19), /* Inactive -> Blocked */
(47, 747, 647, 9,	18,		20), /* Inactive -> Deleted */
(48, 748, 648, 10,	19,		17), /* Blocked -> Active */
(49, 749, 649, 11,	19,		18), /* Blocked -> Inactive */
(50, 750, 650, 12,	19,		20), /* Blocked -> Deleted */

-- Season
(51, 751, 651, 1, NULL,21), /* NULL -> Created */
(52, 752, 652, 2, 21,	22), /* Created -> Open */
(53, 753, 653, 3, 21,	24), /* Created -> Deleted */
(54, 754, 654, 4, 22,	23), /* Open -> Closed */
(55, 755, 655, 5, 22,	24), /* Open -> Deleted */
(56, 756, 656, 6, 23,	22), /* Closed -> Open */
(57, 757, 657, 7, 23,	24), /* Closed -> Deleted */

-- Race
(58, 758, 658, 1, NULL,25), /* NULL -> Created */
(59, 759, 659, 2, 25,	26), /* Created -> Open */
(60, 760, 660, 3, 25,	28), /* Created -> Deleted */
(61, 761, 661, 4, 26,	27), /* Open -> Closed */
(62, 762, 662, 5, 26,	28), /* Open -> Deleted */
(63, 763, 663, 6, 27,	26), /* Closed -> Open */
(64, 764, 664, 7, 27,	28), /* Closed -> Deleted */

-- Result
(65, 765, 665, 1, NULL,29), /* NULL -> Created */
(66, 766, 666, 2, 29,	30), /* Created -> Published */
(67, 767, 667, 3, 29,	31), /* Created -> Deleted */
(68, 768, 668, 4, 30,	31), /* Published -> Deleted */

-- Role
(69, 769, 669, 1, NULL,32), /* NULL -> Created */
(70, 770, 670, 2, 32,	33), /* Created -> Active */
(71, 771, 671, 3, 32,	35), /* Created -> Deleted */
(72, 772, 672, 4, 33,	34), /* Active -> Inactive */
(73, 773, 673, 5, 33,	35), /* Active -> Deleted */
(74, 774, 674, 6, 34,	33), /* Inactive -> Active */
(75, 775, 675, 7, 34,	35), /* Inactive -> Deleted */

-- Template
(76, 776, 676, 1, NULL,36), /* NULL -> Created */
(77, 777, 677, 2, 36,	37), /* Created -> Active */
(78, 778, 678, 3, 36,	39), /* Created -> Deleted */
(79, 779, 679, 4, 37,	38), /* Active -> Inactive */
(80, 780, 680, 5, 37,	39), /* Active -> Deleted */
(81, 781, 681, 6, 38,	37), /* Inactive -> Active */
(82, 782, 682, 7, 38,	39), /* Inactive -> Deleted */

-- Point
(83, 783, 683, 1, NULL,40), /* NULL -> Created */
(84, 784, 684, 2, 40,	41), /* Created -> Active */
(85, 785, 685, 3, 40,	43), /* Created -> Deleted */
(86, 786, 686, 4, 41,	42), /* Active -> Inactive */
(87, 787, 687, 5, 41,	43), /* Active -> Deleted */
(88, 788, 688, 6, 42,	41), /* Inactive -> Active */
(89, 789, 689, 7, 42,	43)  /* Inactive -> Deleted */